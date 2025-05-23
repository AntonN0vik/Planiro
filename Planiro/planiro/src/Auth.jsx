import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';

const Auth = ({ setIsAuthenticated }) => {
  const [isRegistering, setIsRegistering] = useState(true);
  const [formData, setFormData] = useState({
    firstName: '',
    lastName: '',
    username: '',
    password: '',
    confirmPassword: ''
  });
  const [errors, setErrors] = useState({});
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [successMessage, setSuccessMessage] = useState('');
  const navigate = useNavigate();

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: value
    }));

    if (errors[name]) {
      setErrors((prev) => {
        const newErrors = { ...prev };
        delete newErrors[name];
        return newErrors;
      });
    }
  };

  const validateForm = () => {
    const newErrors = {};

    if (isRegistering) {
      if (!formData.firstName.trim()) {
        newErrors.firstName = 'Имя обязательно';
      }
      if (!formData.lastName.trim()) {
        newErrors.lastName = 'Фамилия обязательна';
      }
      if (!formData.username.trim()) {
        newErrors.username = 'Имя пользователя обязательно';
      } else if (formData.username.length < 3) {
        newErrors.username = 'Не менее 3 символов';
      }
    } else {
      if (!formData.username.trim()) {
        newErrors.username = 'Имя пользователя обязательно';
      }
    }

    if (!formData.password) {
      newErrors.password = 'Пароль обязателен';
    } else if (formData.password.length < 6) {
      newErrors.password = 'Пароль должен быть не менее 6 символов';
    }

    if (isRegistering) {
      if (!formData.confirmPassword) {
        newErrors.confirmPassword = 'Подтверждение пароля обязательно';
      } else if (formData.confirmPassword !== formData.password) {
        newErrors.confirmPassword = 'Пароли не совпадают';
      }
    }

    return newErrors;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    const validationErrors = validateForm();

    if (Object.keys(validationErrors).length > 0) {
      setErrors(validationErrors);
      return;
    }

    setIsSubmitting(true);

    try {
      const endpoint = isRegistering
          ? 'http://localhost:5036/api/register'
          : 'http://localhost:5036/api/login';

      const requestData = isRegistering
          ? {
            firstName: formData.firstName,
            lastName: formData.lastName,
            username: formData.username,
            password: formData.password
          }
          : {
            username: formData.username,
            password: formData.password
          };

      const response = await fetch(endpoint, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(requestData),
      });

      const data = await response.json();

      if (!response.ok) {
        throw new Error(data.message || 'Ошибка сервера');
      }

      if (!isRegistering) {
        setIsAuthenticated(true);
        localStorage.setItem('isAuthenticated', 'true');
        navigate('/dev-team');
      }

      setSuccessMessage(isRegistering
          ? 'Регистрация успешна! Теперь вы можете войти.'
          : 'Вход выполнен успешно!');

    } catch (error) {
      setErrors({ server: error.message });
    } finally {
      setIsSubmitting(false);
      setTimeout(() => {
        setSuccessMessage('');
        setFormData({
          firstName: '',
          lastName: '',
          username: '',
          password: '',
          confirmPassword: ''
        });
        setErrors({});
      }, 3000);
    }
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
                  <path d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z" />
                </svg>
              </div>
              <h2>{isRegistering ? 'Создайте свой аккаунт' : 'Добро пожаловать'}</h2>
              <p>
                {isRegistering
                    ? 'Присоединяйтесь к нашему сообществу уже сегодня'
                    : 'Войдите в свой аккаунт и продолжите свое путешествие'}
              </p>
              <button
                  onClick={() => setIsRegistering(!isRegistering)}
                  className="toggle-btn"
              >
                {isRegistering ? 'Уже есть аккаунт? Войти' : 'Нет аккаунта? Зарегистрироваться'}
              </button>
            </div>
          </div>

          {/* Правая панель */}
          <div className="right-panel">
            {successMessage && (
                <div className="success-modal">
                  <div className="success-content">
                    <div className="success-icon">
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
                        <path d="M20 6L9 17l-5-5" />
                      </svg>
                    </div>
                    <h3>Успех!</h3>
                    <p>{successMessage}</p>
                    <button
                        onClick={() => {
                          setSuccessMessage('');
                          setFormData({
                            firstName: '',
                            lastName: '',
                            username: '',
                            password: '',
                            confirmPassword: ''
                          });
                          setErrors({});
                        }}
                        className="btn-primary"
                    >
                      Продолжить
                    </button>
                  </div>
                </div>
            )}

            <form onSubmit={handleSubmit} className="auth-form">
              <h2>{isRegistering ? 'Создайте аккаунт' : 'Войдите в систему'}</h2>

              {isRegistering && (
                  <>
                    <div className="form-group">
                      <label htmlFor="firstName">Имя</label>
                      <input
                          type="text"
                          id="firstName"
                          name="firstName"
                          value={formData.firstName}
                          onChange={handleChange}
                          placeholder="Иван"
                          className={errors.firstName ? 'input-error' : ''}
                      />
                      {errors.firstName && (
                          <span className="error-text">{errors.firstName}</span>
                      )}
                    </div>

                    <div className="form-group">
                      <label htmlFor="lastName">Фамилия</label>
                      <input
                          type="text"
                          id="lastName"
                          name="lastName"
                          value={formData.lastName}
                          onChange={handleChange}
                          placeholder="Иванов"
                          className={errors.lastName ? 'input-error' : ''}
                      />
                      {errors.lastName && (
                          <span className="error-text">{errors.lastName}</span>
                      )}
                    </div>
                  </>
              )}

              <div className="form-group">
                <label htmlFor="username">Имя пользователя</label>
                <input
                    type="text"
                    id="username"
                    name="username"
                    value={formData.username}
                    onChange={handleChange}
                    placeholder="username123"
                    className={errors.username ? 'input-error' : ''}
                />
                {errors.username && (
                    <span className="error-text">{errors.username}</span>
                )}
              </div>

              <div className="form-group">
                <label htmlFor="password">Пароль</label>
                <input
                    type="password"
                    id="password"
                    name="password"
                    value={formData.password}
                    onChange={handleChange}
                    placeholder="••••••••"
                    className={errors.password ? 'input-error' : ''}
                />
                {errors.password && (
                    <span className="error-text">{errors.password}</span>
                )}
              </div>

              {isRegistering && (
                  <div className="form-group">
                    <label htmlFor="confirmPassword">Подтвердите пароль</label>
                    <input
                        type="password"
                        id="confirmPassword"
                        name="confirmPassword"
                        value={formData.confirmPassword}
                        onChange={handleChange}
                        placeholder="••••••••"
                        className={errors.confirmPassword ? 'input-error' : ''}
                    />
                    {errors.confirmPassword && (
                        <span className="error-text">{errors.confirmPassword}</span>
                    )}
                  </div>
              )}

              {errors.server && (
                  <div className="error-text" style={{ textAlign: 'center', margin: '10px 0' }}>
                    {errors.server}
                  </div>
              )}

              <button
                  type="submit"
                  disabled={isSubmitting}
                  className="btn-primary btn-block"
              >
                {isSubmitting
                    ? (isRegistering ? 'Регистрация...' : 'Вход...')
                    : (isRegistering ? 'Зарегистрироваться' : 'Войти')}
              </button>

              <div className="toggle-link">
                <p>
                  {isRegistering ? 'Уже есть аккаунт? ' : 'Нет аккаунта? '}
                  <button
                      type="button"
                      onClick={() => setIsRegistering(!isRegistering)}
                  >
                    {isRegistering ? ' Войти' : ' Зарегистрироваться'}
                  </button>
                </p>
              </div>
            </form>
          </div>
        </div>
      </div>
  );
};

export default Auth;