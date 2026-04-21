# Todo List Application

מערכת לניהול משימות הכוללת צד שרת ב-ASP.NET Core וצד לקוח ב-React. המערכת מאפשרת רישום משתמשים, התחברות מאובטחת וניהול רשימת משימות אישית לכל משתמש.

## טכנולוגיות

### צד שרת (Backend)
- Framework: .NET 8 / ASP.NET Core
- Database: MySQL (Clever Cloud)
- ORM: Entity Framework Core
- Authentication: JWT (JSON Web Tokens)
- Documentation: Swagger / OpenAPI

### צד לקוח (Frontend)
- Library: React
- Routing: React Router Dom
- HTTP Client: Axios
- Styling: CSS3

## מבנה מסד הנתונים

המערכת משתמשת בשתי טבלאות עיקריות בשרת MySQL:
1. Users: שמירת פרטי המשתמשים (ID, Username, Password).
2. items: שמירת המשימות המשויכות למשתמשים (Id, Name, IsComplete, UserId).

## הגדרות והרצה

### צד שרת
1. יש להגדיר את מחרוזת החיבור (Connection String) בקובץ appsettings.json או כמשתנה סביבה ב-Render תחת השם ConnectionStrings__ToDoDB.
2. הגדרת מפתח סודי לאימות JWT תחת Jwt:Key.
3. השרת מוגדר לאפשר CORS עבור הכתובת של ה-Frontend ב-Render.

### צד לקוח
1. יש לעדכן את כתובת ה-Base URL ב-Service של ה-React לכתובת ה-API ב-Render.
2. בפריסה ל-Render, יש להגדיר חוקי Rewrite להפניית כל הבקשות ל-index.html כדי לאפשר ניתוב צד-לקוח (Client-side routing).

## אבטחה
- שימוש ב-Middleware של Authentication ו-Authorization להגנה על נתיבי ה-API.
- מפתחות הצפנה וסיסמאות למסד הנתונים מנוהלים באמצעות Environment Variables ואינם שמורים בקוד המקור.

## פריסה (Deployment)
- Backend: מאוחסן ב-Render כ-Web Service.
- Database: מאוחסן ב-Clever Cloud כ-MySQL Add-on.
- Frontend: מאוחסן ב-Render כ-Static Site.

## ניטור
הפרויקט כולל רכיב מוניטור פנימי המבוסס על Node.js המאפשר מעקב אחר סטטוס השירותים דרך ה-API של Render.