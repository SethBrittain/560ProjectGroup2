package com.TeamTwo.DatabaseProject.modules.security;

import org.springframework.beans.factory.annotation.Value;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.security.config.annotation.web.builders.HttpSecurity;
import org.springframework.security.config.annotation.web.configuration.EnableWebSecurity;
import org.springframework.security.oauth2.core.DelegatingOAuth2TokenValidator;
import org.springframework.security.oauth2.core.OAuth2TokenValidator;
import org.springframework.security.oauth2.jwt.Jwt;
import org.springframework.security.oauth2.jwt.JwtDecoder;
import org.springframework.security.oauth2.jwt.JwtDecoders;
import org.springframework.security.oauth2.jwt.JwtValidators;
import org.springframework.security.oauth2.jwt.NimbusJwtDecoder;
import org.springframework.security.web.SecurityFilterChain;

@EnableWebSecurity
@Configuration
public class SecurityConfig {

    // @Value("${auth0.audience}")
    // private String audience;

    // @Value("${spring.security.oauth2.resourceserver.jwt.issuer-uri}")
    // private String issuer;

    // @Bean
    // JwtDecoder jwtDecoder() {
    //     NimbusJwtDecoder jwtDecoder = (NimbusJwtDecoder)
    //             JwtDecoders.fromOidcIssuerLocation(issuer);

    //     OAuth2TokenValidator<Jwt> audienceValidator = (OAuth2TokenValidator<Jwt>) new AudienceValidator(audience);
    //     OAuth2TokenValidator<Jwt> withIssuer = JwtValidators.createDefaultWithIssuer(issuer);
    //     OAuth2TokenValidator<Jwt> withAudience = new DelegatingOAuth2TokenValidator<>(withIssuer, audienceValidator);

    //     jwtDecoder.setJwtValidator(withAudience);

    //     return jwtDecoder;
    // }

    @Bean
    public SecurityFilterChain filterChain(HttpSecurity http) throws Exception {
        // http.authorizeRequests()
        //         .requestMatchers("*").authenticated()
        //         .and().cors()
        //         .and().oauth2ResourceServer().jwt();
        return http.build();
    }
}