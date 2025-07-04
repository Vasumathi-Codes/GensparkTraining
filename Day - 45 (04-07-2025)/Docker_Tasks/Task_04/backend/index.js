// index.js
const express = require('express');
const app = express();
const PORT = 5001;

app.get('/api/hello', (req, res) => {
  res.json({ message: 'Hello from Backend!' });
});

app.listen(PORT, '0.0.0.0', () => {
  console.log(`Backend API running on port ${PORT}`);
});
