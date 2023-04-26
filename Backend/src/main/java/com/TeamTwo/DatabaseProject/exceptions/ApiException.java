package com.TeamTwo.DatabaseProject.exceptions;

import org.springframework.http.HttpStatus;
import org.springframework.web.bind.annotation.ResponseStatus;
 
@ResponseStatus(HttpStatus.NOT_FOUND)
public class ApiException extends RuntimeException
{
    private static final long serialVersionUID = 1L;
 
    public ApiException(String message) {
        super(message);
    }
}