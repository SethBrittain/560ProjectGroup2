package com.TeamTwo.DatabaseProject;

import org.springframework.context.annotation.Configuration;
import org.springframework.web.servlet.config.annotation.CorsRegistry;
import org.springframework.web.servlet.config.annotation.EnableWebMvc;
import org.springframework.web.servlet.config.annotation.WebMvcConfigurer;

@Configuration
@EnableWebMvc
public class WebConfig implements WebMvcConfigurer {
    @Override
    public void addCorsMappings(CorsRegistry registry) {
		/* ************************* Reaally bad ************************* */
		/* TODO: REMOVE BEFORE USING IN ANY SORT OF PRODUCTION ENVIRONMENT */
        registry.addMapping("/api/**")
            .allowedOrigins("*")
            .allowedMethods("GET", "PUT", "POST", "DELETE");
    }
}