import React, {useState, useEffect} from 'react';
import {DndContext, closestCenter} from '@dnd-kit/core';
import {
    SortableContext,
    verticalListSortingStrategy,
    arrayMove
} from '@dnd-kit/sortable';
import {useSortable} from '@dnd-kit/sortable';
import {CSS} from '@dnd-kit/utilities';
import axios from 'axios';

const SortableTask = ({task}) => {
    const {
        attributes,
        listeners,
        setNodeRef,
        transform,
        transition
    } = useSortable({
        id: task.id
    });

    const style = {
        transform: CSS.Transform.toString(transform),
        transition,
        cursor: 'grab'
    };

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
                <span>{task.assignee}</span>
                <span>{task.deadline}</span>
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
    const [newTask, setNewTask] = useState({
        title: '',
        description: '',
        deadline: '',
        assignee: ''
    });

    useEffect(() => {
        const teamId = localStorage.getItem('teamId');
        axios.get(`/api/teams/${teamId}`)
            .then(response => {
                setTasks(response.data.tasks);
                setMembers(response.data.members);
            });
    }, []);

    const handleDragEnd = (event) => {
        const {active, over} = event;
        if (!over || active.id === over.id) return;

        setTasks((tasks) => {
            const oldIndex = tasks.findIndex(t => t.id === active.id);
            const newIndex = tasks.findIndex(t => t.id === over.id);

            const updatedTasks = arrayMove(tasks, oldIndex, newIndex);
            axios.put('/api/tasks/reorder', {tasks: updatedTasks});

            return updatedTasks;
        });
    };

    const handleCreateTask = () => {
        axios.post('/api/tasks', newTask)
            .then(() => {
                setShowTaskModal(false);
                // Обновить список задач
            });
    };

    const getTasksByColumn = (column) =>
        tasks.filter(task => task.status === column);

    const handleModalClick = (e) => {
        if (e.target.classList.contains('modal')) {
            setShowCodeModal(false);
            setShowTaskModal(false);
        }
    };

    return (
        <div className="team-board">
            {/* Верхняя панель (без изменений) */}
            <div className="top-panel">
                <select onChange={(e) => setViewMode(e.target.value)}>
                    <option value="my">Мои задачи</option>
                    <option value="all">Все задачи</option>
                    <option value="user">Задачи пользователя</option>
                </select>

                {isTeamLead && (
                    <div className="team-lead-controls">
                        <button onClick={() => setShowCodeModal(true)}>Код
                            команды
                        </button>
                        <button onClick={() => setShowTaskModal(true)}>+ Новая
                            задача
                        </button>
                    </div>
                )}
            </div>

            {/* Основной контент */}
            <div className="board-content">
                {/* Панель участников (без изменений) */}
                <div className="members-panel">
                    <h3>Участники</h3>
                    <ul>
                        {members.map(member => (
                            <li key={member.id}>
                                {member.name}
                                {isTeamLead && <button
                                    onClick={() => handleRemoveMember(member.id)}>×</button>}
                            </li>
                        ))}
                    </ul>
                </div>

                {/* Доска задач */}
                <DndContext
                    collisionDetection={closestCenter}
                    onDragEnd={handleDragEnd}
                >
                    <div className="task-board">
                        {['To Do', 'Doing', 'Checking', 'Done'].map((column) => (
                            <div className="task-column" key={column}>
                                <h4>{column}</h4>
                                <SortableContext
                                    items={getTasksByColumn(column)}
                                    strategy={verticalListSortingStrategy}
                                >
                                    {getTasksByColumn(column).map(task => (
                                        <SortableTask key={task.id}
                                                      task={task}/>
                                    ))}
                                </SortableContext>
                            </div>
                        ))}
                    </div>
                </DndContext>
            </div>

            {/* Модальные окна (без изменений) */}
            {showCodeModal && (
                <div className="modal" onClick={handleModalClick}>
                    <div className="modal-content">
                        <h3>Код
                            команды: {localStorage.getItem('teamCode')}</h3>
                        <button className="modal-btn primary code-close-btn"
                                onClick={() => setShowCodeModal(false)}>Закрыть
                        </button>
                    </div>
                </div>
            )}

            {showTaskModal && (
                <div className="modal" onClick={handleModalClick}>
                    <div className="modal-content">
                        <div className="modal-header">
                            <h3>Создать новую задачу</h3>
                        </div>

                        <div className="modal-body">
                            <input
                                className="modal-input"
                                placeholder="Название задачи"
                                onChange={(e) => setNewTask({
                                    ...newTask,
                                    title: e.target.value
                                })}
                            />

                            <textarea
                                className="modal-input"
                                placeholder="Описание"
                                rows="3"
                                onChange={(e) => setNewTask({
                                    ...newTask,
                                    description: e.target.value
                                })}
                            />

                            <input
                                type="date"
                                className="modal-input"
                                onChange={(e) => setNewTask({
                                    ...newTask,
                                    deadline: e.target.value
                                })}
                            />

                            <select
                                className="modal-input"
                                onChange={(e) => setNewTask({
                                    ...newTask,
                                    assignee: e.target.value
                                })}
                            >
                                <option value="">Выберите исполнителя</option>
                                {members.map(member => (
                                    <option key={member.id}
                                            value={member.id}>{member.name}</option>
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