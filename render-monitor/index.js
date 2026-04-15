// const express = require('express');
// // const sdk = require('@api/render-api');
// const sdk = require('api')('@render-api/v1.0#972625ls09s78k');
// const app = express();
// const port = 3000;

// // כאן אנחנו מחברים את המפתח הסודי שלך
// // ב-Render אנחנו נגדיר משתנה סביבה בשם RENDER_API_KEY
// sdk.auth(process.env.RENDER_API_KEY || 'YOUR_TEMPORARY_KEY_FOR_LOCAL_TEST');

// app.get('/services', (req, res) => {
//     // הפקודה הזו פונה ל-API של Render ומבקשת את רשימת השירותים
//     sdk.listServices({limit: '20'})
//         .then(({ data }) => {
//             console.log("Success!");
//             res.json(data); // מחזיר את רשימת האפליקציות שלך כ-JSON
//         })
//         .catch(err => {
//             console.error(err);
//             res.status(500).json({ error: "Failed to fetch services" });
//         });
// });

// app.listen(port, () => {
//     console.log(`Monitor app listening at http://localhost:${port}`);
// });
const express = require('express');
const axios = require('axios');
const app = express();

app.get('/services', async (req, res) => {
    try {
        const response = await axios.get('https://api.render.com/v1/services', {
            headers: {
                'Authorization': `Bearer ${process.env.RENDER_API_KEY}`,
                'Accept': 'application/json'
            }
        });
        
        console.log("Success! Data retrieved.");
        res.json(response.data);
    } catch (err) {
        console.error("Error details:", err.response ? err.response.data : err.message);
        res.status(500).json({ 
            error: "Failed to fetch services", 
            details: err.message 
        });
    }
});

const port = process.env.PORT || 3000;
app.listen(port, () => console.log(`Server running on port ${port}`));