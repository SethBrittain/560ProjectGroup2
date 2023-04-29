package com.TeamTwo.DatabaseProject;

import java.io.IOException;

import org.springframework.context.annotation.Configuration;
import org.springframework.core.annotation.Order;
import org.springframework.http.HttpMethod;
import org.springframework.stereotype.Component;
import org.springframework.web.servlet.config.annotation.CorsRegistry;
import org.springframework.web.servlet.config.annotation.EnableWebMvc;
import org.springframework.web.servlet.config.annotation.WebMvcConfigurer;

import jakarta.servlet.Filter;
import jakarta.servlet.FilterChain;
import jakarta.servlet.FilterConfig;
import jakarta.servlet.ServletException;
import jakarta.servlet.ServletRequest;
import jakarta.servlet.ServletResponse;
import jakarta.servlet.http.HttpServletResponse;
import jakarta.servlet.http.HttpServletRequest;

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
// @Component
// public class WebConfig implements Filter {

//     @Override
//     public void doFilter(ServletRequest req, ServletResponse res, FilterChain chain) throws IOException, ServletException {
//         final HttpServletResponse response = (HttpServletResponse) res;
//         response.setHeader("Access-Control-Allow-Origin", "*");
//         response.setHeader("Access-Control-Allow-Methods", "POST, PUT, GET, OPTIONS, DELETE");
//         response.setHeader("Access-Control-Allow-Headers", "Authorization, Content-Type, enctype");
//         response.setHeader("Access-Control-Max-Age", "3600");
        
//         if (HttpMethod.OPTIONS.name().equalsIgnoreCase(((HttpServletRequest)req).getMethod())) {
//             response.setStatus(HttpServletResponse.SC_OK);
//         } else {
//             chain.doFilter(req, res);
//         }
//     }

//     @Override
//     public void destroy() {
//     }

//     @Override
//     public void init(FilterConfig config) throws ServletException {
//     }
// }