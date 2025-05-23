import React from 'react';
import { useNavigate } from 'react-router-dom';

const DevTeamPage = () => {
    const navigate = useNavigate();

    const handleLogout = () => {
        localStorage.removeItem('isAuthenticated');
        navigate('/auth');
    };

    return (
        <div className="dev-team-page">
            <button
                onClick={handleLogout}
                className="btn-primary"
                style={{ float: 'right', margin: '20px' }}
            >
                Выйти
            </button>
            <h1>Введите код команды разработки</h1>
            <form className="dev-team-form">
                <input
                    type="text"
                    placeholder="Код команды"
                    className="form-input"
                />
                <button
                    type="submit"
                    className="btn-primary btn-block"
                >
                    Подтвердить
                </button>
            </form>
        </div>
    );
};

export default DevTeamPage;