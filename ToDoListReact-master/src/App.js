import React, { useEffect, useState } from 'react';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import service from './service.js';
import Login from './Login';
import Register from './Register';
import './style.css'

function App() {
  const [newTodo, setNewTodo] = useState("");
  const [todos, setTodos] = useState([]);
  // const [user, setUser] = useState()
  const userName = localStorage.getItem("userName");
  // בדיקה אם המשתמש מחובר על סמך קיום טוקן ב-LocalStorage
  const [isLoggedIn, setIsLoggedIn] = useState(!!localStorage.getItem("token"));

  async function getTodos() {
    try {
      const todos = await service.getTasks();
      setTodos(todos);
    } catch (error) {
      console.error("טעינת המשימות נכשלה");
    }
  }

  // פונקציית התנתקות
  const handleLogout = () => {
    localStorage.removeItem("token");
    setIsLoggedIn(false);
  };

  useEffect(() => {
    if (isLoggedIn) {
      getTodos();
    }
  }, [isLoggedIn]);

  // --- לוגיקת המשימות המקורית שלך (הוספה, עדכון, מחיקה) ---
  async function createTodo(e) {
    e.preventDefault();
    await service.addTask(newTodo);
    setNewTodo("");
    await getTodos();
  }

  async function updateCompleted(todo, isComplete) {
    await service.setCompleted(todo.id, isComplete, todo.name);
    await getTodos();
  }

  async function deleteTodo(id) {
    await service.deleteTask(id);
    await getTodos();
  }

  return (
    <BrowserRouter>
      <Routes>
        {/* דף הרשמה */}
        <Route path="/register" element={<Register onRegisterSuccess={() => window.location.href = '/login'} />} />

        {/* דף התחברות */}
        <Route path="/login" element={
          isLoggedIn ? <Navigate to="/" /> : <Login onLoginSuccess={() => setIsLoggedIn(true)} />
        } />

        {/* הדף הראשי - מוגן ע"י בדיקת התחברות */}
        <Route path="/" element={
          !isLoggedIn ? <Navigate to="/login" /> : (
            <section className="todoapp">
              <h5>שלום {userName}</h5>
              <header className="header">
                <button onClick={handleLogout} style={{float: 'right', margin: '10px'}}>התנתק</button>
                <h1>todos</h1>
                <form onSubmit={createTodo}>
                  <input 
                    className="new-todo" 
                    placeholder="Well, let's take on the day" 
                    value={newTodo} 
                    onChange={(e) => setNewTodo(e.target.value)} 
                  />
                </form>
              </header>
              <section className="main" style={{ display: "block" }}>
                <ul className="todo-list">
                  {todos.map(todo => (
                    <li className={todo.isComplete ? "completed" : ""} key={todo.id}>
                      <div className="view">
                        <input 
                          className="toggle" 
                          type="checkbox" 
                          checked={todo.isComplete} 
                          onChange={(e) => updateCompleted(todo, e.target.checked)} 
                        />
                        <label>{todo.name}</label>
                        <button className="destroy" onClick={() => deleteTodo(todo.id)}></button>
                      </div>
                    </li>
                  ))}
                </ul>
              </section>
            </section>
          )
        } />
      </Routes>
    </BrowserRouter>
  );
}

export default App;