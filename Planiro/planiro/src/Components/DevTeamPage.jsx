import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';

const DevTeamPage = () => {
    const navigate = useNavigate();
    const [isCreating, setIsCreating] = useState(false);
    const [code, setCode] = useState('');
    const [error, setError] = useState('');

    const createTeam = async () => {
        try {
            setIsCreating(true);
            setError('');

            const response = await fetch('http://localhost:5036/api/Teams/create', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    username: localStorage.getItem('username')
                })
            });

            const data = await response.json();

            if (!response.ok) throw new Error(data.message);

            navigate('/team-code', { state: { teamCode: data.code } });
        } catch (error) {
            setError('Ошибка при создании команды: ' + error.message);
        } finally {
            setIsCreating(false);
        }
    };

    const joinTeam = async (e) => {
        e.preventDefault();
        try {
            const response = await fetch('http://localhost:5036/api/Teams/join', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    code,
                    username: localStorage.getItem('username')
                })
            });

            const data = await response.json();
            if (!response.ok) throw new Error(data.message);

            alert('Успешное подключение к команде!');
        } catch (error) {
            setError('Ошибка подключения: ' + error.message);
        }
    };

    return (
        <div className="auth-container">
            <div className="auth-card">
                <div className="left-panel">
                    <div className="overlay"></div>
                    <div className="content">
                        <div className="icon-wrapper">
                            <svg
                                xmlns="http://www.w3.org/2000/svg"
                                width="24"
                                height="24"
                                viewBox="0 0 24 24"
                                fill="none"
                                stroke="currentColor"
                                strokeWidth="2"
                                strokeLinecap="round"
                                strokeLinejoin="round"
                            >
                                <path d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z" />
                            </svg>
                        </div>
                        <h2>Команда разработки</h2>
                        <p>Введите код команды для доступа к совместной работе</p>
                    </div>
                </div>

                <div className="right-panel">
                    <form className="auth-form" onSubmit={joinTeam}>
                        <h2>Доступ к команде</h2>

                        <div className="form-group">
                            <label>Код команды</label>
                            <input
                                type="text"
                                value={code}
                                onChange={(e) => setCode(e.target.value)}
                                placeholder="XXXX-XXXX"
                                className="form-input"
                            />
                        </div>

                        <button
                            type="submit"
                            className="btn-primary btn-block"
                        >
                            Подтвердить
                        </button>

                        <div className="team-actions">
                            <button
                                type="button"
                                onClick={createTeam}
                                className="btn-primary btn-block"
                                disabled={isCreating}
                            >
                                {isCreating ? 'Создание...' : 'Создать новую команду'}
                            </button>
                            {error && <div className="error-text">{error}</div>}
                        </div>
                    </form>
                </div>
            </div>
        </div>
    );
};

export default DevTeamPage;