import React, { useState, useEffect, useRef } from 'react';
import './CSS/ChatPage.css'; // Importăm fișierul CSS pentru animație
import { useUser } from "../../../context/LoginRequired";
import { PaperAirplaneIcon } from '@heroicons/react/24/solid';
import ConversationHistory from './components/ConversationHistory'; // Importăm componenta SideNav

const ChatPage = () => {
  const [conversations, setConversations] = useState([]);
  const [currentConversationId, setCurrentConversationId] = useState(null);
  const [messages, setMessages] = useState([]);
  const [input, setInput] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const [userPhotoUrl, setUserPhotoUrl] = useState('');
  const messagesEndRef = useRef(null);
  const user = useUser();
  const s3BaseUrl = "https://galabucket.s3.eu-north-1.amazonaws.com/";

  useEffect(() => {
    const fetchUserPhoto = async () => {
        try {
            const response = await fetch(`https://localhost:7115/api/cloud/get-user-photo?userId=${user.userId}`);
            const data = await response.json();
            if (data.success) {
                setUserPhotoUrl(`${s3BaseUrl}${data.userPhotoUrl}`);
            }
        } catch (error) {
            console.error("Error fetching user photo:", error);
        }
    };

    fetchUserPhoto();

    // Recuperăm conversațiile specifice utilizatorului din Local Storage la încărcarea componentului
    loadConversations();

    // Inițiem o conversație nouă la încărcarea paginii
    startNewConversation();
}, [user.userId]);


  const startNewConversation = () => {
    const newConversation = {
      id: Date.now(),
      messages: [],
      name: '' // Lasăm numele gol la început
    };
    setConversations(prevConversations => [...prevConversations, newConversation]);
    setCurrentConversationId(newConversation.id);
    setMessages([]);
  };

  const saveConversation = (newMessages, newName = null) => {
    const updatedConversations = conversations.map(conv => {
        if (conv.id === currentConversationId) {
            return { ...conv, messages: newMessages, name: newName ? newName : conv.name };
        }
        return conv;
    });
    setConversations(updatedConversations);
    localStorage.setItem(`conversations_${user.userId}`, JSON.stringify(updatedConversations));
};

const loadConversations = () => {
    const storedConversations = JSON.parse(localStorage.getItem(`conversations_${user.userId}`)) || [];
    setConversations(storedConversations);
};

const deleteConversation = (conversationId) => {
    const updatedConversations = conversations.filter(conv => conv.id !== conversationId);
    setConversations(updatedConversations);
    localStorage.setItem(`conversations_${user.userId}`, JSON.stringify(updatedConversations));
  
    if (currentConversationId === conversationId) {
        startNewConversation();
    }
};

const handleConversationClick = (conversationId) => {
    setCurrentConversationId(conversationId);
    const selectedConversation = conversations.find(conv => conv.id === conversationId);
    setMessages(selectedConversation ? selectedConversation.messages : []);
};

  const generateConversationName = (message) => {
    const words = message.split(' ');
    if (words.length <= 3) {
      return message;
    }
    return words.slice(0, 3).join(' ') + '...';
  };

  const handleSend = async () => {
    if (input.trim() === '') return; 

    setIsLoading(true); 
    const currentInput = input; 
    setInput(''); 
    try {
      const response = await fetch(`https://localhost:7115/api/v1/recommendations?userRequest=${encodeURIComponent(currentInput)}`);
      const data = await response.text(); 
      const newMessages = [...messages, { user: currentInput, assistant: data }];
      setMessages(newMessages);

      if (messages.length === 0) {
        const newName = generateConversationName(currentInput);
        saveConversation(newMessages, newName);
      } else {
        saveConversation(newMessages);
      }
    } catch (error) {
      console.error("Error sending message:", error);
    } finally {
      setIsLoading(false); 
      scrollToBottom();
    }
  };

  const handlePromptClick = (prompt) => {
    setInput(prompt);
  };

  const handleKeyDown = (event) => {
    if (event.key === 'Enter') {
      handleSend();
    }
  };

  const scrollToBottom = () => {
    messagesEndRef.current?.scrollIntoView({ behavior: "smooth" });
  };

  useEffect(() => {
    scrollToBottom();
  }, [messages]);

  const defaultPrompts = [
    "Which is the most popular book?",
    "What books do you recommend?",
    "What authors do you recommend?"
  ];

  return (
    <div className="flex container">
      <ConversationHistory
        conversations={conversations}
        currentConversationId={currentConversationId}
        onConversationClick={handleConversationClick}
        onNewConversation={startNewConversation}
        onDeleteConversation={deleteConversation}
      />
      <div className="flex flex-col flex-grow bg-surface-darker-white text-white rounded-r-lg shadow-lg">
        <header className="bg-surface-dark-green p-4 shadow-md rounded-t-lg">
          <h1 className="text-xl font-semibold text-center">Chat with Wall-e</h1>
        </header>
        <main className="flex-grow p-4 overflow-y-auto flex flex-col">
          {messages.length === 0 && !isLoading ? (
            <div className="text-center flex flex-col items-center justify-center flex-grow">
              <img src="/img/AiAssistant.PNG" alt="Bot Logo" className="w-24 h-24 rounded-full mb-4" />
              <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
                {defaultPrompts.map((prompt, index) => (
                  <button
                    key={index}
                    onClick={() => handlePromptClick(prompt)}
                    className="bg-surface-dark-green p-4 rounded-lg hover:bg-surface-light-green transition"
                  >
                    {prompt}
                  </button>
                ))}
              </div>
            </div>
          ) : (
            <div className="flex flex-col space-y-4 flex-grow overflow-y-auto">
              {messages.map((msg, index) => (
                <React.Fragment key={index}>
                  <div className="flex items-start space-x-4 bg-transparent text-black p-4 rounded-lg">
                    <img src={userPhotoUrl} alt="User Avatar" className="w-12 h-12 rounded-full" />
                    <div className="flex flex-col">
                      <div className="text-surface-dark-green font-bold">{user.name}:</div>
                      <div>{msg.user}</div>
                    </div>
                  </div>
                  <div className="flex items-start space-x-4 bg-transparent text-black p-4 rounded-lg">
                    <img src="/img/AiAssistant.PNG" alt="Assistant Avatar" className="w-12 h-12 rounded-full" />
                    <div className="flex flex-col">
                      <div className="text-surface-dark-green font-bold">Wall-e:</div>
                      <div>{msg.assistant}</div>
                    </div>
                  </div>
                </React.Fragment>
              ))}
              {isLoading && (
                <div className="flex justify-center items-center mt-4">
                  <span className="dot bg-surface-dark-green rounded-full w-2 h-2 mx-1 animate-bounce"></span>
                  <span className="dot bg-surface-dark-green rounded-full w-2 h-2 mx-1 animate-bounce delay-200"></span>
                  <span className="dot bg-surface-dark-green rounded-full w-2 h-2 mx-1 animate-bounce delay-400"></span>
                </div>
              )}
              <div ref={messagesEndRef} />
            </div>
          )}
        </main>
        <footer className="bg-surface-dark-green p-4 rounded-b-lg">
          <div className="flex items-center space-x-4">
            <input
              type="text"
              value={input}
              onChange={(e) => setInput(e.target.value)}
              onKeyDown={handleKeyDown} // Adaugă handler-ul pentru apăsarea tastei Enter
              className="flex-grow border p-2 rounded-lg bg-surface-white outline-none text-surface-black"
              placeholder="Type your message..."
            />
            <button
              onClick={handleSend}
              className="bg-surface-dark-green text-white p-2 rounded-lg flex items-center justify-center"
            >
              <PaperAirplaneIcon className="h-5 w-5 transform rotate-45" /> {/* Iconița de trimitere */}
            </button>
          </div>
        </footer>
      </div>
    </div>
  );
};

export default ChatPage;