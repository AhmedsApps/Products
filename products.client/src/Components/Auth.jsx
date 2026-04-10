import React, { useState } from 'react';

export default function Auth({ onLoginSuccess }) {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [message, setMessage] = useState('');

    const handleLogin = async (e) => {
        e.preventDefault();
        setMessage('');

        try {
            // In a real application, I would handle this more securely and not hardcode the URL
            const response = await fetch('https://localhost:7266/api/auth/login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ username, password })
            });

            if (response.ok) {
                const data = await response.json();
                // Store the token in localStorage for the sake of this example, but will use more secured storage in production
                localStorage.setItem('token', data.token);
                setMessage('Login successful');
                onLoginSuccess?.(data.token);
            } else {
                setMessage('Login failed');
            }
        } catch (error) {
            setMessage('Login failed due to an error: ' + error.message);
        }
    };

    return (
        <div style={{ maxWidth: '400px', margin: '50px auto', padding: '20px', border: '1px solid #ccc', borderRadius: '8px' }}>
            <h2>Login</h2>
            {message && <div style={{ color: 'red', marginBottom: '15px', padding: '10px', backgroundColor: '#ffe6e6', borderRadius: '4px' }}>{message}</div>}
            <form onSubmit={handleLogin} style={{ display: 'flex', flexDirection: 'column' }}>
                    <div>
                        <label htmlFor="username" style={{ display: 'block', marginBottom: '5px' }}>Username</label>
                        <input
                            type="text"
                            placeholder="Username"
                            value={username}
                            required
                            style={{ width: '100%', padding: '8px', boxSizing: 'border-box' }}
                            onChange={(e) => setUsername(e.target.value)}
                        />
                        <label htmlFor="password" style={{ display: 'block', marginBottom: '5px' }}>Password</label>
                        <input
                            type="password"
                            placeholder="Password"
                            value={password}
                            required
                            style={{ width: '100%', padding: '8px', boxSizing: 'border-box' }}
                            onChange={(e) => setPassword(e.target.value)}
                        />
                        <button onClick={handleLogin}>Login</button>
                    </div>               
            </form>
        </div>
    );
}