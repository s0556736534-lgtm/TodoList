import { useState } from 'react';
import service from './service';
import { Link } from 'react-router-dom'; 

function Register({ onRegisterSuccess }) {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [message, setMessage] = useState("");
  const [error, setError] = useState("");

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");
    setMessage("");

    try {
      await service.register(username, password);
      setMessage("ההרשמה בוצעה בהצלחה! כעת ניתן להתחבר.");
      // אפשר להוסיף כאן השהיה קצרה ואז להעביר לדף לוגין
      if (onRegisterSuccess) {
          setTimeout(() => onRegisterSuccess(), 2000);
      }
    } catch (err) {
      setError(err.response?.data || "שגיאה בתהליך ההרשמה");
    }
  };

  return (
    <div className="login-container">
      <h2>יצירת משתמש חדש</h2>
      <form onSubmit={handleSubmit}>
        <div>
          <label>שם משתמש:</label>
          <input 
            type="text" 
            value={username} 
            onChange={(e) => setUsername(e.target.value)} 
            required 
          />
        </div>
        <div>
          <label>סיסמה:</label>
          <input 
            type="password" 
            value={password} 
            onChange={(e) => setPassword(e.target.value)} 
            required 
          />
        </div>
        {error && <p style={{ color: 'red' }}>{error}</p>}
        {message && <p style={{ color: 'green' }}>{message}</p>}
        <button type="submit">הירשם</button>
      </form>
      <p style={{ marginTop: '15px' }}>
        כבר רשום? <Link to="/Login">תתחבר כאן</Link>
      </p>
    </div>
  );
}

export default Register;