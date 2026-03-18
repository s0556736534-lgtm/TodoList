import axios from 'axios';

// הגדרת כתובת ה-API כברירת מחדל (Config Defaults)
axios.defaults.baseURL = process.env.REACT_APP_API_URL || "http://localhost:5235";

// הוספת Interceptor לתפיסת שגיאות ב-Response
axios.interceptors.response.use(
    response => response, // אם התגובה תקינה, פשוט החזר אותה
    error => {
        // רישום השגיאה ללוג באופן גלובלי
        console.error('משהו השתבש בקריאת ה-API:', {
            message: error.message,
            status: error.response?.status,
            data: error.response?.data
        });
        return Promise.reject(error); // העברת השגיאה הלאה כדי שהקומפוננטה תוכל להגיב לה
    }
);

// הוספת ה-Token לכל בקשה יוצאת באופן אוטומטי
axios.interceptors.request.use(config => {
    const token = localStorage.getItem("token");
    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
});

export default {
    // שליפת כל המשימות
    getTasks: async () => {
        const result = await axios.get("/api/items");
        return result.data;
    },

    // הוספת משימה חדשה
    addTask: async (name) => {
        console.log('addTask', name);
        const result = await axios.post("/api/items", {
            name: name,
            isComplete: false
        });
        return result.data;
    },

    // עדכון סטטוס משימה
    setCompleted: async (id, isComplete, name) => {
        console.log('setCompleted', { id, isComplete });
        const result = await axios.put(`/api/items/${id}`, {
            name: name,
            isComplete: isComplete
        });
        return result.data;
    },


    // מחיקת משימה
    deleteTask: async (id) => {
        console.log('deleteTask', id);
        const result = await axios.delete(`/api/items/${id}`);
        return result.data;
    },

    // בתוך האובייקט שאתה מייצא ב-service.js:
    login: async (username, password) => {
        // שליחת בקשה לשרת
        const result = await axios.post("/api/auth/login", {
            username,
            password
        });

        // שמירת הטוקן שהתקבל מהשרת בזיכרון המקומי של הדפדפן
        if (result.data && result.data.token) {
            localStorage.setItem("token", result.data.token);
        }
        return result.data;
    },
    // בתוך האובייקט ב-service.js
    register: async (username, password) => {
        const result = await axios.post("/api/auth/register", {
            username,
            password
        });
        return result.data;
    }
};