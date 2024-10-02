import React, { useState, useEffect, useCallback } from 'react';
import { HubConnectionBuilder, HttpTransportType, LogLevel } from '@microsoft/signalr';

const UserChats = () => {
    const [connection, setConnection] = useState(null);
    const [messages, setMessages] = useState([]);
    const [room, setRoom] = useState('');
    const [user, setUser] = useState('');

    // Initialize the connection when the component mounts
    useEffect(() => {
        const connect = new HubConnectionBuilder()
            .withUrl('YOUR_SIGNALR_HUB_URL', {
                transport: HttpTransportType.WebSockets,
                skipNegotiation: true,
                accessTokenFactory: () => 'Your access token if needed'
            })
            .configureLogging(LogLevel.Information)
            .build();

        setConnection(connect);

        return () => {
            if (connect) {
                connect.stop();
            }
        };
    }, []);

    // Start the connection and add listeners
    useEffect(() => {
        if (connection) {
            connection.start()
                .then(() => {
                    console.log('Connection started!');

                    // Listen for messages
                    connection.on('ReceiveMessage', (user, message, timestamp) => {
                        setMessages(prevMessages => [...prevMessages, { user, message, timestamp }]);
                    });

                    // Listen for changes in the connected users
                    connection.on('ConnectedUser', (users) => {
                        console.log('Connected users:', users);
                    });
                })
                .catch(err => console.error('Connection failed: ', err));
        }
    }, [connection]);

    const joinRoom = async () => {
        if (connection) {
            const userRoomConnection = { user, room };
            await connection.invoke('JoinRoom', userRoomConnection);
        }
    };

    const leaveRoom = async () => {
        if (connection) {
            await connection.invoke('LeaveRoom', { user, room });
        }
    };

    const sendMessage = async (message) => {
        if (connection) {
            await connection.invoke('SendMessage', message);
        }
    };

    return (
        <div>
            <input value={user} onChange={(e) => setUser(e.target.value)} placeholder="Your name" />
            <input value={room} onChange={(e) => setRoom(e.target.value)} placeholder="Room" />
            <button onClick={joinRoom}>Join Room</button>
            <button onClick={leaveRoom}>Leave Room</button>
            <ul>
                {messages.map((msg, index) => (
                    <li key={index}>{msg.user}: {msg.message} - {new Date(msg.timestamp).toLocaleTimeString()}</li>
                ))}
            </ul>
            <input
                type="text"
                onKeyDown={(e) => {
                    if (e.key === 'Enter') {
                        sendMessage(e.target.value);
                        e.target.value = '';
                    }
                }}
                placeholder="Type a message..."
            />
        </div>
    );
};

export default UserChats;
