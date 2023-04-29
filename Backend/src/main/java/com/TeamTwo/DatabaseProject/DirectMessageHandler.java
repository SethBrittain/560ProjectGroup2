package com.TeamTwo.DatabaseProject;

import java.util.ArrayList;
import java.util.HashMap;

import org.springframework.web.socket.CloseStatus;
import org.springframework.web.socket.TextMessage;
import org.springframework.web.socket.WebSocketMessage;
import org.springframework.web.socket.WebSocketSession;
import org.springframework.web.socket.handler.TextWebSocketHandler;

public class DirectMessageHandler extends TextWebSocketHandler {
    HashMap<Integer,ArrayList<WebSocketSession>> webSocketSessions = new HashMap<Integer,ArrayList<WebSocketSession>>();
    
    @Override
    public void afterConnectionEstablished(WebSocketSession session) throws Exception {
        super.afterConnectionEstablished(session);
        Integer directId = (Integer)session.getAttributes().get("directId");
        ArrayList<WebSocketSession> channelSessions = webSocketSessions.get(directId);
        if (channelSessions == null) {
            ArrayList<WebSocketSession> newSessionList = new ArrayList<WebSocketSession>();
            newSessionList.add(session);
            webSocketSessions.put(directId, newSessionList);
            System.out.println(String.format("Opened Session on DirectId %d", directId));
        }
    }

    @Override
    public void afterConnectionClosed(WebSocketSession session, CloseStatus status) throws Exception {
        super.afterConnectionClosed(session, status);
        
        Integer directId = (Integer)session.getAttributes().get("directId");
        ArrayList<WebSocketSession> channelSessions = webSocketSessions.get(directId);
        if (channelSessions != null && channelSessions.contains(session)) {
            channelSessions.remove(session);
            if (channelSessions.size() < 1) {
                webSocketSessions.remove(directId);
                System.out.println(String.format("Closed Session on directId %d", directId));
            }
        }
    }

    @Override
    public void handleMessage(WebSocketSession session, WebSocketMessage<?> message) throws Exception {
        super.handleMessage(session, message);
        System.out.println("Send message session attributes: " + session.getAttributes());
        for (WebSocketSession webSocketSession : webSocketSessions.get((Integer)session.getAttributes().get("directId"))) {
            webSocketSession.sendMessage(new TextMessage("message"));
        }
    }
}