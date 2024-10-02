import React from 'react';
import { TrashIcon, PencilSquareIcon } from '@heroicons/react/24/solid';

const ConversationHistory = ({ conversations, currentConversationId, onConversationClick, onNewConversation, onDeleteConversation }) => {
  return (
    <aside className="flex flex-col bg-surface-dark-green text-white p-4 rounded-l-lg shadow-lg h-full w-64">
      <h2 className="text-xl font-semibold mb-4 text-center">Conversations</h2>
      <ul className="flex-grow overflow-y-auto space-y-2">
        {conversations.map((conv) => (
          <li key={conv.id} className="flex justify-between items-center p-2 rounded-lg bg-surface-dark-green hover:bg-surface-light-green active:bg-surface-light-green ">
            <div
              className={`flex-grow cursor-pointer ${conv.id === currentConversationId ? 'bg-transparent' : ''}`}
              onClick={() => onConversationClick(conv.id)}
            >
              {conv.name || ``}
            </div>
            {conv.messages.length > 0 && (
              <button
                className="ml-2 p-1 rounded-lg bg-transparent transition"
                onClick={() => onDeleteConversation(conv.id)}
              >
                <TrashIcon className="h-5 w-5 text-red-600" />
              </button>
            )}
          </li>
        ))}
      </ul>
      <button className="mt-4 p-2 bg-surface-dark-green rounded-lg hover:bg-surface-light-green transition w-full" onClick={onNewConversation}>
        <PencilSquareIcon className="h-6 w-6 text-white mx-auto" />
      </button>
      {}
      <div className="xl:w-52 md:w-40 w-36 invisible"></div>
    </aside>
  );
};

export default ConversationHistory;
