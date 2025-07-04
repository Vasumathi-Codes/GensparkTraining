const express = require('express');
const { MongoClient } = require('mongodb');
const app = express();

const PORT = 5005;
const MONGO_URL = process.env.MONGO_URL;

app.get('/', async (req, res) => {
  try {
    const client = await MongoClient.connect(MONGO_URL);
    const db = client.db('db');
    const collections = await db.collections();
    res.json({ message: 'Connected to MongoDB!', collections: collections.map(c => c.collectionName) });
    client.close();
  } catch (error) {
    res.status(500).json({ error: 'Failed to connect to MongoDB', details: error.message });
  }
});

app.listen(PORT, () => {
  console.log(`Node API running on port ${PORT}`);
});
