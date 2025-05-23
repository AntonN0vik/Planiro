import React, { useState } from 'react';

export default function App() {
  const [isRegistering, setIsRegistering] = useState(true);
  const [formData, setFormData] = useState({
    email: '',
    password: '',
    confirmPassword: '',
    name: ''
  });
  const [errors, setErrors] = useState({});
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [successMessage, setSuccessMessage] = useState('');

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

    if (!formData.email) {
      newErrors.email = 'Email обязателен';
    } else if (!/\S+@\S+\.\S+/.test(formData.email)) {
      newErrors.email = 'Неверный формат email';
    }

    if (!formData.password) {
      newErrors.password = 'Пароль обязателен';
    } else if (formData.password.length < 6) {
      newErrors.password = 'Пароль должен быть не менее 6 символов';
    }

    if (isRegistering) {
      if (!formData.name) {
        newErrors.name = 'Имя обязательно';
      }

      if (!formData.confirmPassword) {
        newErrors.confirmPassword = 'Подтверждение пароля обязательно';
      } else if (formData.confirmPassword !== formData.password) {
        newErrors.confirmPassword = 'Пароли не совпадают';
      }
    }

    return newErrors;
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    const validationErrors = validateForm();

    if (Object.keys(validationErrors).length > 0) {
      setErrors(validationErrors);
      return;
    }

    setIsSubmitting(true);

    setTimeout(() => {
      setIsSubmitting(false);
      if (isRegistering) {
        setSuccessMessage('Регистрация успешна! Теперь вы можете войти.');
        
      } else {
        setSuccessMessage('Вход выполнен успешно!');
      }

      setTimeout(() => {
        setSuccessMessage('');
        setFormData({
          email: '',
          password: '',
          confirmPassword: '',
          name: ''
        });
        setErrors({});
      }, 3000);
    }, 1500);
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
            <button onClick={() => setIsRegistering(!isRegistering)} className="toggle-btn">
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
                    setFormData({ email: '', password: '', confirmPassword: '', name: '' });
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
              <div className="form-group">
                <label htmlFor="name">Полное имя</label>
                <input
                  type="text"
                  id="name"
                  name="name"
                  value={formData.name}
                  onChange={handleChange}
                  placeholder="Иван Иванов"
                  className={errors.name ? 'input-error' : ''}
                />
                {errors.name && <span className="error-text">{errors.name}</span>}
              </div>
            )}

            <div className="form-group">
              <label htmlFor="email">Email адрес</label>
              <input
                type="email"
                id="email"
                name="email"
                value={formData.email}
                onChange={handleChange}
                placeholder="example@example.com"
                className={errors.email ? 'input-error' : ''}
              />
              {errors.email && <span className="error-text">{errors.email}</span>}
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
              {errors.password && <span className="error-text">{errors.password}</span>}
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
                {errors.confirmPassword && <span className="error-text">{errors.confirmPassword}</span>}
              </div>
            )}

            <button type="submit" disabled={isSubmitting} className="btn-primary btn-block">
              {isSubmitting ? (isRegistering ? 'Регистрация...' : 'Вход...') : isRegistering ? 'Зарегистрироваться' : 'Войти'}
            </button>

            <div className="toggle-link">
              <p>
                {isRegistering ? 'Уже есть аккаунт? ' : 'Нет аккаунта? '}
                <button type="button" onClick={() => setIsRegistering(!isRegistering)}>
                  {isRegistering ? ' Войти' : ' Зарегистрироваться'}
                </button>
              </p>
            </div>
          </form>
        </div>
      </div>

      <style>
        {`
          * {
            box-sizing: border-box;
            margin: 0;
            padding: 0;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
          }

          body {
            background: linear-gradient(135deg, #f0f4f8, #d9e2ec);
            min-height: 100vh;
            display: flex;
            align-items: center;
            justify-content: center;
            padding: 20px;
          }

          .auth-container {
            width: 100%;
            max-width: 900px;
            border-radius: 16px;
            overflow: hidden;
            box-shadow: 0 10px 30px rgba(0, 0, 0, 0.1);
            display: flex;
            flex-direction: row;
            background-color: #fff;
          }

          .auth-card {
            display: flex;
            flex-direction: row;
            width: 100%;
          }

          .left-panel {
            position: relative;
            background: linear-gradient(135deg, #4f46e5, #3730a3);
            color: white;
            padding: 40px;
            display: flex;
            align-items: center;
            justify-content: center;
            flex: 1;
          }

          .left-panel .overlay {
            position: absolute;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            transform: skewY(-12deg);
            transform-origin: top left;
            background: linear-gradient(135deg, #4f46e5, #3730a3);
            z-index: 0;
          }

          .left-panel .content {
            position: relative;
            z-index: 1;
            text-align: center;
            max-width: 300px;
          }

          .icon-wrapper {
            width: 60px;
            height: 60px;
            background: rgba(255, 255, 255, 0.2);
            border-radius: 50%;
            display: flex;
            align-items: center;
            justify-content: center;
            margin: 0 auto 20px;
          }

          .icon-wrapper svg {
            color: white;
            width: 24px;
            height: 24px;
          }

          .left-panel h2 {
            font-size: 24px;
            font-weight: bold;
            margin-bottom: 10px;
          }

          .left-panel p {
            font-size: 14px;
            margin-bottom: 20px;
            opacity: 0.9;
          }

          .toggle-btn {
            background: white;
            color: #4f46e5;
            border: none;
            padding: 10px 20px;
            border-radius: 8px;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.3s ease;
          }

          .toggle-btn:hover {
            background: #e0e7ff;
          }

          .right-panel {
            flex: 1;
            padding: 40px;
            position: relative;
          }

          .right-panel h2 {
            font-size: 24px;
            font-weight: bold;
            margin-bottom: 30px;
            color: #1f2937;
          }

          .auth-form {
            display: flex;
            flex-direction: column;
            gap: 20px;
          }

          .form-group {
            display: flex;
            flex-direction: column;
          }

          .form-group label {
            font-size: 14px;
            margin-bottom: 6px;
            color: #4b5563;
          }

          .form-group input {
            padding: 12px 14px;
            border: 1px solid #d1d5db;
            border-radius: 8px;
            font-size: 14px;
            transition: border-color 0.3s ease;
          }

          .form-group input:focus {
            outline: none;
            border-color: #818cf8;
            box-shadow: 0 0 0 3px rgba(129, 140, 248, 0.2);
          }

          .input-error {
            border-color: #ef4444;
          }

          .error-text {
            color: #ef4444;
            font-size: 12px;
            margin-top: 4px;
            display: block;
          }

          .btn-primary {
            background: #4f46e5;
            color: white;
            border: none;
            padding: 12px;
            border-radius: 8px;
            font-size: 16px;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.3s ease;
          }

          .btn-primary:hover {
            background: #3730a3;
          }

          .btn-block {
            width: 100%;
          }

          .toggle-link {
            text-align: center;
            margin-top: 10px;
          }

          .toggle-link button {
            background: none;
            border: none;
            color: #4f46e5;
            font-weight: 600;
            cursor: pointer;
          }

          .success-modal {
            position: absolute;
            inset: 0;
            background: rgba(255, 255, 255, 0.95);
            backdrop-filter: blur(4px);
            display: flex;
            align-items: center;
            justify-content: center;
            z-index: 10;
            animation: fadeIn 0.4s ease-in-out;
            border-radius: 16px;
          }

          .success-content {
            text-align: center;
            padding: 30px;
          }

          .success-icon {
            width: 60px;
            height: 60px;
            background: #dcfce7;
            border-radius: 50%;
            display: flex;
            align-items: center;
            justify-content: center;
            margin: 0 auto 20px;
            color: #16a34a;
          }

          @keyframes fadeIn {
            from {
              opacity: 0;
              transform: translateY(10px);
            }
            to {
              opacity: 1;
              transform: translateY(0);
            }
          }

          @media (max-width: 768px) {
            .auth-card {
              flex-direction: column;
            }
          }
        `}
      </style>
    </div>
  );
}