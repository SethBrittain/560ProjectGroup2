package com.TeamTwo.DatabaseProject;

import java.util.ArrayList;
import java.util.Collections;
import java.util.HashMap;
import java.util.List;
import org.springframework.web.socket.CloseStatus;
import org.springframework.web.socket.TextMessage;
import org.springframework.web.socket.WebSocketMessage;
import org.springframework.web.socket.WebSocketSession;
import org.springframework.web.socket.handler.TextWebSocketHandler;
import com.mashape.unirest.http.JsonNode;

public class ChannelMessageHandler extends TextWebSocketHandler {
    List<WebSocketSession> webSocketSessions = Collections.synchronizedList(new ArrayList<>());
    //HashMap<ArrayList<String>,List<WebSocketSession>> webSocketSessions = new HashMap<ArrayList<String>,List<WebSocketSession>>();
    
    @Override
    public void afterConnectionEstablished(WebSocketSession session) throws Exception {
        super.afterConnectionEstablished(session);
        System.out.println(session.getAttributes());
    }

    @Override
    public void afterConnectionClosed(WebSocketSession session, CloseStatus status) throws Exception {
        System.out.println("Closed");
        super.afterConnectionClosed(session, status);
        webSocketSessions.remove(session);
        System.out.println(this.webSocketSessions.size());
    }

    @Override
    public void handleMessage(WebSocketSession session, WebSocketMessage<?> message) throws Exception {
        // ArrayList<String> strarr1 = new ArrayList<String>(List.of("1","2"));
        // ArrayList<String> strarr2 = new ArrayList<String>(List.of("1","2"));
        
        super.handleMessage(session, message);
        for (WebSocketSession webSocketSession : webSocketSessions) {
            webSocketSession.sendMessage(new TextMessage("message"));
        }
    }

}