package com.TeamTwo.DatabaseProject;

import java.util.ArrayList;
import java.util.HashMap;
import org.springframework.web.socket.CloseStatus;
import org.springframework.web.socket.TextMessage;
import org.springframework.web.socket.WebSocketMessage;
import org.springframework.web.socket.WebSocketSession;
import org.springframework.web.socket.handler.TextWebSocketHandler;

public class ChannelMessageHandler extends TextWebSocketHandler {
    HashMap<Integer,ArrayList<WebSocketSession>> webSocketSessions = new HashMap<Integer,ArrayList<WebSocketSession>>();
    
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
        System.out.println("Got to handle message");
        ArrayList<WebSocketSession> sessionGroup = webSocketSessions.get(getSessionGroupId(session));
        System.out.println("------- DEBUG -------");
        System.out.println(session.getAttributes().get("channelId"));
        System.out.println(webSocketSessions.get(getSessionGroupId(session)));
        System.out.println(sessionGroup.size());
        if (sessionGroup != null) {
            System.out.println("SessionGroup not null");
            for (WebSocketSession sess : sessionGroup) {
                String finalMessage = "Sent message from " + session.getId() + " to " + sess.getId() + ": " + message.getPayload();
                sess.sendMessage(new TextMessage(finalMessage));
            }
        }
    }

    private Integer getSessionGroupId(WebSocketSession session) {
        return (Integer)session.getAttributes().get("channelId");
    }
}