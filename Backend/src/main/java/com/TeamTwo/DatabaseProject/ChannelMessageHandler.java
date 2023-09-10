package com.TeamTwo.DatabaseProject;

import java.util.ArrayList;
import java.util.HashMap;

import org.json.JSONObject;
import org.springframework.web.socket.CloseStatus;
import org.springframework.web.socket.TextMessage;
import org.springframework.web.socket.WebSocketMessage;
import org.springframework.web.socket.WebSocketSession;
import org.springframework.web.socket.handler.TextWebSocketHandler;

import com.TeamTwo.DatabaseProject.modules.user.database.UserDatabase;

public class ChannelMessageHandler extends TextWebSocketHandler {
    HashMap<Integer,ArrayList<WebSocketSession>> webSocketSessions = new HashMap<Integer,ArrayList<WebSocketSession>>();
    
    private UserDatabase database;

	public ChannelMessageHandler(UserDatabase db) {
		this.database = db;
	}

    @Override
    public void afterConnectionEstablished(WebSocketSession session) throws Exception {
        super.afterConnectionEstablished(session);
        
        System.out.println("Connect Session Attributes: " + session.getAttributes());
        Integer channelId = (Integer)session.getAttributes().get("channelId");

        ArrayList<WebSocketSession> channelSessions = webSocketSessions.get(channelId);
        if (channelSessions == null) {
            ArrayList<WebSocketSession> newSessionList = new ArrayList<WebSocketSession>();
            newSessionList.add(session);
            webSocketSessions.put(channelId, newSessionList);
        } else {
            channelSessions.add(session);
        }
    }

    @Override
    public void afterConnectionClosed(WebSocketSession session, CloseStatus status) throws Exception {
        super.afterConnectionClosed(session, status);
        
        Integer channelId = (Integer)session.getAttributes().get("channelId");
        ArrayList<WebSocketSession> channelSessions = webSocketSessions.get(channelId);
        if (channelSessions != null && channelSessions.contains(session)) {
            channelSessions.remove(session);
            if (channelSessions.size() < 1) {
                webSocketSessions.remove(channelId);
                System.out.println(String.format("Closed Session on channelId %d", channelId));
            }
        }
    }

    @Override
    public void handleMessage(WebSocketSession session, WebSocketMessage<?> message) throws Exception {
        super.handleMessage(session, message);

        // Get conversation group
        ArrayList<WebSocketSession> sessionGroup = webSocketSessions.get(getSessionGroupId(session));
        
        // Get user id and channel id from connection attributes
        JSONObject attr = new JSONObject(session.getAttributes());
        int channelId = attr.getInt("channelId");
        int userId = this.database.GetUserId(attr.getString("apiKey"));

        // Get the message sent as a singular string
        JSONObject msg = new JSONObject(message.getPayload().toString());
        String messageText = msg.get("message").toString();
        
        int insertedMsgId = this.database.InsertMessageIntoChannel(messageText, userId, channelId);
        System.out.println("msg id: " + insertedMsgId);
        System.out.println("user id: " + userId);
        JSONObject finalMessage = this.database.GetMessageById(insertedMsgId, userId);

        if (sessionGroup != null) {
            for (WebSocketSession sess : sessionGroup) {
                sess.sendMessage(new TextMessage(finalMessage.toString()));
            }
        }
    }

    private Integer getSessionGroupId(WebSocketSession session) {
        return (Integer)session.getAttributes().get("channelId");
    }
}