package com.TeamTwo.DatabaseProject;

import java.util.Map;

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

@Configuration
@EnableWebSocket
public class WebSocketConfig implements WebSocketConfigurer {

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
                String pathPrefix = "/direct/";
                
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
                String path = serverHttpRequest.getURI().getPath();
                String pathPrefix = "/channel/";
                
                String idString = path.substring(path.indexOf(pathPrefix)+pathPrefix.length());
                int id = Integer.parseInt(idString);
                map.put("channelId", id);
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
    public ChannelMessageHandler channelWebSocketHandler() {
        return new ChannelMessageHandler();
    }

    @Bean
    public WebSocketContainerFactoryBean createWebSocketContainer() {
        WebSocketContainerFactoryBean container = new WebSocketContainerFactoryBean();
        container.setMaxTextMessageBufferSize(1024);
        container.setMaxBinaryMessageBufferSize(1024);
        container.setMaxSessionIdleTimeout(8192000);
        container.setAsyncSendTimeout(8192000);
        return container;
    }

    @Override
    public void registerWebSocketHandlers(WebSocketHandlerRegistry webSocketHandlerRegistry) {
        webSocketHandlerRegistry.addHandler(new DirectMessageHandler(), "/direct/{token}")
            .setAllowedOrigins("*").addInterceptors(DirectMessageInterceptor());
        webSocketHandlerRegistry.addHandler(new ChannelMessageHandler(), "/channel/{token}")
            .setAllowedOrigins("*").addInterceptors(ChannelMessageInterceptor());
    }
}