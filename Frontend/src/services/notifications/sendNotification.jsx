import axios from 'axios';

export async function sendNotification(userId, message) {
  const url = 'https://localhost:7115/api/v1/inboxitems/send-notification';
  const data = {
    userId: userId,
    message: message
  };

  try {
    const response = await axios.post(url, data, {
      headers: {
        'Content-Type': 'application/json',
        // Adaugă orice alte headere necesare, cum ar fi token-ul de autentificare
      }
    });

    if (response.status === 200) {
      console.log('Notification sent successfully:', response.data);
      return response.data;
    } else {
      console.error('Failed to send notification:', response.status, response.data);
      return null;
    }
  } catch (error) {
    console.error('Error sending notification:', error);
    return null;
  }
}


export async function fetchNotifications(userId, token) {
  try {
    const response = await api.get(`/api/v1/InboxItems/get-by-user-id/${userId}`, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });
    if (response.status !== 200) {
      throw new Error(response);
    }
    return response.data ? response.data.inboxItems : null; // Verificați dacă există response.data
  } catch (error) {
    console.log(`Error while getting notifications: ${error.response.data}`);
  }
}
