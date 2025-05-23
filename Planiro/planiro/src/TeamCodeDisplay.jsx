import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';

const TeamCodeDisplay = ({ teamCode }) => {
    const navigate = useNavigate();
    const [isCopied, setIsCopied] = useState(false);

    const copyToClipboard = () => {
        navigator.clipboard.writeText(teamCode);
        setIsCopied(true);
        setTimeout(() => setIsCopied(false), 2000);
    };

    return (
        <div className="auth-container">
            <div className="auth-card">
                {/* Левая панель */}
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
                                <path d="M16 4h2a2 2 0 0 1 2 2v14a2 2 0 0 1-2 2H6a2 2 0 0 1-2-2V6a2 2 0 0 1 2-2h2"></path>
                                <rect x="8" y="2" width="8" height="4" rx="1" ry="1"></rect>
                            </svg>
                        </div>
                        <h2>Команда создана!</h2>
                        <p>
                            Используйте этот код для доступа к вашей команде
                        </p>
                    </div>
                </div>

                {/* Правая панель */}
                <div className="right-panel">
                    <div className="auth-form">
                        <h2>Код вашей команды</h2>
                        <div className="code-display">
                            <span className="code">{teamCode}</span>
                            <button
                                onClick={copyToClipboard}
                                className="btn-primary copy-btn"
                            >
                                {isCopied ? 'Скопировано!' : 'Копировать'}
                            </button>
                        </div>
                        <button
                            onClick={() => navigate('/dev-team')}
                            className="btn-primary btn-block"
                        >
                            Войти в команду
                        </button>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default TeamCodeDisplay;