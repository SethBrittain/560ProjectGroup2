package com.TeamTwo.DatabaseProject;

import java.util.Map;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.http.server.*;
import org.springframework.web.socket.server.*;
import org.springframework.web.socket.WebSocketHandler;
import org.springframework.web.socket.client.*;
import org.springframework.web.socket.client.standard.WebSocketContainerFactoryBean;
import org.springframework.web.socket.config.annotation.EnableWebSocket;
import org.springframework.web.socket.config.annotation.WebSocketConfigurer;
import org.springframework.web.socket.config.annotation.WebSocketHandlerRegistry;
import org.springframework.web.socket.server.HandshakeInterceptor;

import com.TeamTwo.DatabaseProject.modules.user.database.UserDatabase;

@Configuration
@EnableWebSocket
public class WebSocketConfig implements WebSocketConfigurer {

    private UserDatabase database;

    @Autowired
    public WebSocketConfig(UserDatabase db)
    {
        this.database = db;
    }

    private HandshakeInterceptor DirectMessageInterceptor() {
        return new HandshakeInterceptor() {
            @Override
            public boolean beforeHandshake (
                ServerHttpRequest serverHttpRequest,
                ServerHttpResponse serverHttpResponse,
                WebSocketHandler webSocketHandler,
                Map<String, Object> map
            ) throws Exception {
                String path = serverHttpRequest.getURI().getPath();
                String pathPrefix = "/ws/direct/";
                
                String idString = path.substring(path.indexOf(pathPrefix)+pathPrefix.length());
                int id = Integer.parseInt(idString);
                map.put("directId", id);
                return true;
            }

            @Override
            public void afterHandshake(
                ServerHttpRequest request, 
                ServerHttpResponse response,
                WebSocketHandler wsHandler, 
                Exception exception
            ) {}
        };
    }
    private HandshakeInterceptor ChannelMessageInterceptor() {
        return new HandshakeInterceptor() {
            @Override
            public boolean beforeHandshake (
                ServerHttpRequest serverHttpRequest,
                ServerHttpResponse serverHttpResponse,
                WebSocketHandler webSocketHandler,
                Map<String, Object> map
            ) throws Exception {
                String path = serverHttpRequest.getURI().getPath().substring(1);
                String[] urlParams = path.split("/");
                System.out.println(urlParams.length);
                if (urlParams.length > 3) {
                    map.put("channelId", Integer.parseInt(urlParams[2]));
                    map.put("apiKey", urlParams[3]);
                }
                System.out.println(map.toString());

                return true;
            }

            @Override
            public void afterHandshake(
                ServerHttpRequest request, 
                ServerHttpResponse response,
                WebSocketHandler wsHandler, 
                Exception exception
            ) {}
        };
    }

    @Bean
    public DirectMessageHandler directWebSocketHandler() {
        return new DirectMessageHandler();
    }

    @Bean
    public WebSocketContainerFactoryBean createWebSocketContainer() {
        WebSocketContainerFactoryBean container = new WebSocketContainerFactoryBean();
        container.setMaxTextMessageBufferSize(128000000);
        container.setMaxBinaryMessageBufferSize(128000000);
        container.setMaxSessionIdleTimeout(8192000);
        container.setAsyncSendTimeout(8192000);
        return container;
    }

    @Override
    public void registerWebSocketHandlers(WebSocketHandlerRegistry webSocketHandlerRegistry) {
        webSocketHandlerRegistry.addHandler(new DirectMessageHandler(), "/wss/direct/{id}/{token}")
            .setAllowedOrigins("*").addInterceptors(DirectMessageInterceptor());
        webSocketHandlerRegistry.addHandler(new ChannelMessageHandler(database), "/wss/channel/{id}/{token}")
            .setAllowedOrigins("*").addInterceptors(ChannelMessageInterceptor());
    }
}
