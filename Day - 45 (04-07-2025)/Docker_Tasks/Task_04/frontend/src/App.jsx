import { useEffect, useState } from 'react';

function App() {
  const [message, setMessage] = useState('');

  useEffect(() => {
    fetch('http://backend:5001/api/hello')
      .then(res => res.json())
      .then(data => setMessage(data.message))
      .catch(err => {
        console.error('API call failed:', err);
        setMessage('Could not reach backend ');
      });
  }, []);

  return (
    <div style={{ textAlign: 'center', marginTop: '5rem' }}>
      <h1>React Frontend</h1>
      <p>{message} hehe</p>
    </div>
  );
}

export default App;
