import React, {useState, useEffect} from 'react';
import {
    DndContext,
    closestCenter,
    DragOverlay,
    useSensor,
    useSensors,
    PointerSensor,
    useDroppable
} from '@dnd-kit/core';
import {restrictToVerticalAxis} from '@dnd-kit/modifiers'
import {
    SortableContext,
    verticalListSortingStrategy,
    arrayMove,
    useSortable
} from '@dnd-kit/sortable';
import {CSS} from '@dnd-kit/utilities';
import axios from 'axios';

const API_URL = 'http://localhost:5036/api';

const SortableTask = ({task, members}) => {
    const {
        attributes,
        listeners,
        setNodeRef,
        transform,
        transition,
        isDragging
    } = useSortable({
        id: task.id
    });

    const style = {
        transform: CSS.Transform.toString(transform),
        transition,
        cursor: 'grab',
        opacity: isDragging ? 0.5 : 1
    };

    // Находим имя исполнителя
    const assigneeName = members.find(m => m.id === task.assignee)?.name || 'Не назначен';

    return (
        <div
            ref={setNodeRef}
            style={style}
            {...attributes}
            {...listeners}
            className="task-card"
        >
            <h5>{task.title}</h5>
            <p>{task.description}</p>
            <div className="task-footer">
                <span>{assigneeName}</span>
                <span>{new Date(task.deadline).toLocaleDateString()}</span>
            </div>
        </div>
    );
};

// Компонент для области сброса (колонки)
const DroppableColumn = ({column, tasks, members}) => {
    const {setNodeRef, isOver} = useDroppable({
        id: column
    });

    return (
        <div
            ref={setNodeRef}
            className={`task-column ${isOver ? 'droppable-over' : ''}`}
            data-column={column}
        >
            <div className="column-header">
                <h4>{column} ({tasks.length})</h4>
            </div>
            <div className="task-list">
                <SortableContext
                    items={tasks.map(task => task.id)}
                    strategy={verticalListSortingStrategy}
                >
                    {tasks.map((task) => (
                        <SortableTask
                            key={task.id}
                            task={task}
                            members={members}
                        />
                    ))}
                </SortableContext>
            </div>
        </div>
    );
};


const TeamBoard = ({isTeamLead}) => {
    const [tasks, setTasks] = useState([]);
    const [members, setMembers] = useState([]);
    const [viewMode, setViewMode] = useState('my');
    const [showCodeModal, setShowCodeModal] = useState(false);
    const [showTaskModal, setShowTaskModal] = useState(false);
    const [activeTask, setActiveTask] = useState(null);
    const [newTask, setNewTask] = useState({
        title: '',
        description: '',
        assignee: '',
        deadline: '',
        status: ''
    });

    // Настройка сенсоров для drag & drop
    const sensors = useSensors(
        useSensor(PointerSensor, {
            activationConstraint: {
                distance: 8,
            },
        })
    );

    // Загрузка данных
    useEffect(() => {
        const loadData = async () => {
            try {
                const teamId = localStorage.getItem('teamId');
                const response = await axios.get(`${API_URL}/Teams/${teamId}`);
                setTasks(response.data.tasks);
                setMembers(response.data.members);
            } catch (error) {
                console.error('Ошибка загрузки данных:', error);
            }
        };
        loadData();
    }, []);

    // Обработка начала перетаскивания
    const handleDragStart = (event) => {
        const {active} = event;
        const task = tasks.find(t => t.id === active.id);
        setActiveTask(task);
    };

    // Обработка окончания перетаскивания
    const handleDragEnd = async (event) => {
        const {active, over} = event;
        setActiveTask(null);

        if (!over || activeTask.status === over.id) {
            return; // Ничего не делаем
        }

        const taskId = active.id;
        const targetColumn = over.id;

        const originalTasks = [...tasks];
        try {
            // Оптимистичное обновление
            const updatedTasks = tasks.map(t =>
                t.id === taskId ? {...t, status: targetColumn} : t
            );
            setTasks(updatedTasks);
            let current_teamId = localStorage.getItem('teamId')

            // Отправка запроса
            await axios.put(`${API_URL}/Tasks/${current_teamId}/${taskId}`, {
                ...tasks.find(t => t.id === taskId),
                status: targetColumn // Добавлено
            });
        } catch (error) {
            console.error('Ошибка перемещения задачи:', error);
            setTasks(originalTasks); // Откат при ошибке
        }
    };

    // Создание задачи
    const handleCreateTask = async () => {
        try {
            let current_teamId = localStorage.getItem('teamId')

            const response = await axios.post(`${API_URL}/Tasks/${current_teamId}`, {
                ...newTask,
                status: 'To Do', // Добавлено
            });

            setTasks([...tasks, response.data]);
            setShowTaskModal(false);
            setNewTask({
                title: '',
                description: '',
                assignee: '',
                deadline: '',
                status: ''
            });
        } catch (error) {
            console.error('Ошибка создания задачи:', error);
        }
    };

    // Удаление участника
    const handleRemoveMember = async (memberId) => {
        try {
            let current_teamId = localStorage.getItem('teamId')
            await axios.delete(`${API_URL}/Members/${current_teamId}/${memberId}`);
            setMembers(members.filter(m => m.id !== memberId));
        } catch (error) {
            console.error('Ошибка удаления участника:', error);
        }
    };

    // Фильтрация задач
    const getTasksByColumn = (column) => {
        return tasks.filter(task =>
            task.status === column &&
            (viewMode === 'all' ||
                (viewMode === 'my' && task.assignee === 'current-user-id') ||
                (viewMode === 'user' && task.assignee === 'selected-user-id')))
    };

    // Закрытие модалок
    const handleModalClick = (e) => {
        if (e.target.classList.contains('modal')) {
            setShowCodeModal(false);
            setShowTaskModal(false);
        }
    };

    const columns = ['To Do', 'Doing', 'Checking', 'Done'];

    return (
        <div className="team-board">
            {/* Верхняя панель */}
            <div className="top-panel">
                <select
                    value={viewMode}
                    onChange={(e) => setViewMode(e.target.value)}
                    className="view-selector"
                >
                    <option value="my">Мои задачи</option>
                    <option value="all">Все задачи</option>
                    <option value="user">Задачи пользователя</option>
                </select>

                {isTeamLead && (
                    <div className="team-lead-controls">
                        <button
                            className="team-code-btn"
                            onClick={() => setShowCodeModal(true)}
                        >
                            Код команды
                        </button>
                        <button
                            className="new-task-btn"
                            onClick={() => setShowTaskModal(true)}
                        >
                            + Новая задача
                        </button>
                    </div>
                )}
            </div>

            {/* Основной контент */}
            <div className="board-content">
                {/* Панель участников */}
                <div className="members-panel">
                    <h3>Участники ({members.length})</h3>
                    <ul>
                        {members.map(member => (
                            <li key={member.id}>
                                <span
                                    className="member-name">{member.name}</span>
                                {isTeamLead && (
                                    <button
                                        className="remove-member-btn"
                                        onClick={() => handleRemoveMember(member.id)}
                                    >
                                        ×
                                    </button>
                                )}
                            </li>
                        ))}
                    </ul>
                </div>

                {/* Доска задач */}
                <DndContext
                    sensors={sensors}
                    collisionDetection={closestCenter}
                    onDragStart={handleDragStart}
                    onDragEnd={handleDragEnd}
                >
                    <div className="task-board">
                        {columns.map((column) => (
                            <DroppableColumn
                                key={column}
                                column={column}
                                tasks={getTasksByColumn(column)}
                                members={members}
                            />
                        ))}
                    </div>

                    {/* Оверлей для перетаскиваемого элемента */}
                    <DragOverlay>
                        {activeTask ? (
                            <div className="task-card dragging">
                                <h5>{activeTask.title}</h5>
                                <p>{activeTask.description}</p>
                                <div className="task-footer">
                                    <span>{members.find(m => m.id === activeTask.assignee)?.name || 'Не назначен'}</span>
                                    <span>{new Date(activeTask.deadline).toLocaleDateString()}</span>
                                </div>
                            </div>
                        ) : null}
                    </DragOverlay>
                </DndContext>
            </div>

            {/* Модальные окна */}
            {showCodeModal && (
                <div className="modal" onClick={handleModalClick}>
                    <div className="modal-content code-modal">
                        <h3>Код
                            команды: {localStorage.getItem('teamCode')}</h3>
                        <button
                            className="modal-btn primary"
                            onClick={() => setShowCodeModal(false)}
                        >
                            Закрыть
                        </button>
                    </div>
                </div>
            )}

            {showTaskModal && (
                <div className="modal" onClick={handleModalClick}>
                    <div className="modal-content task-modal">
                        <div className="modal-header">
                            <h3>Создать новую задачу</h3>
                        </div>

                        <div className="modal-body">
                            <input
                                className="modal-input"
                                placeholder="Название задачи"
                                value={newTask.title}
                                onChange={(e) => setNewTask({
                                    ...newTask,
                                    title: e.target.value
                                })}
                            />

                            <textarea
                                className="modal-input"
                                placeholder="Описание"
                                rows="3"
                                value={newTask.description}
                                onChange={(e) => setNewTask({
                                    ...newTask,
                                    description: e.target.value
                                })}
                            />

                            <input
                                type="date"
                                className="modal-input"
                                value={newTask.deadline}
                                onChange={(e) => setNewTask({
                                    ...newTask,
                                    deadline: e.target.value
                                })}
                            />

                            <select
                                className="modal-input"
                                value={newTask.assignee}
                                onChange={(e) => setNewTask({
                                    ...newTask,
                                    assignee: e.target.value
                                })}
                            >
                                <option value="">Выберите исполнителя</option>
                                {members.map(member => (
                                    <option key={member.id} value={member.id}>
                                        {member.name}
                                    </option>
                                ))}
                            </select>
                        </div>

                        <div className="modal-footer">
                            <button
                                className="modal-btn secondary"
                                onClick={() => setShowTaskModal(false)}
                            >
                                Отмена
                            </button>
                            <button
                                className="modal-btn primary"
                                onClick={handleCreateTask}
                                disabled={!newTask.title.trim()}
                            >
                                Создать задачу
                            </button>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
};

export default TeamBoard;