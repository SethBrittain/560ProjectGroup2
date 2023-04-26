package com.TeamTwo.DatabaseProject;

import java.sql.Connection;
import java.sql.SQLException;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.core.env.Environment;

import com.mashape.unirest.http.HttpResponse;
import com.mashape.unirest.http.JsonNode;
import com.mashape.unirest.http.Unirest;
import com.mashape.unirest.http.exceptions.UnirestException;

import ch.qos.logback.classic.Logger;

@Configuration
public class ApiConfig {
    @Autowired
    private Environment env;

    @Bean(name = "ApiTokenObject")
    public Object token()
	{
        String bodyString = String.format("{\"client_id\":\"%1$s\",\"client_secret\":\"%2$s\",\"audience\":\"%3$s\",\"grant_type\":\"client_credentials\"}",env.getProperty("api.management.clientid"),env.getProperty("api.management.secret"),env.getProperty("api.management.audience"));
        HttpResponse<JsonNode> response;
        
        try {
            response = Unirest.post(this.env.getProperty("api.management.tokenurl"))
                .header("content-type", "application/json")
                .body(bodyString)
                .asJson();
            
            return response.getBody().getObject().get("access_token");
        } catch (UnirestException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
            return new Object();
        }
	}

}
