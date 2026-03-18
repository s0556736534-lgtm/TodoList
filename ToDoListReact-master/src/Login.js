import React, { useState } from 'react';
import service from './service';
import { Link } from 'react-router-dom'; 

function Login({ onLoginSuccess }) {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError(""); // איפוס שגיאות קודמות

    try {
      await service.login(username, password);
      onLoginSuccess(); // פונקציה שתעדכן את מצב האפליקציה למחובר
    } catch (err) {
      setError("שם משתמש או סיסמה שגויים");
      console.error("Login failed", err);
    }
  };

  return (
    <div className="login-container">
      <h2>התחברות למערכת</h2>
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
        <button type="submit">התחבר</button>
      </form>
      <p style={{ marginTop: '15px' }}>
        עדיין אין לך חשבון? <Link to="/register">הירשם כאן</Link>
      </p>
    </div>
  );
}

export default Login;