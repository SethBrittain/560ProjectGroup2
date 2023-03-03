package com.TeamTwo.DatabaseProject.modules.user.controller;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RestController;

import com.TeamTwo.DatabaseProject.modules.user.database.UserDatabase;

@RestController
public class UserController {

    private UserDatabase database; 

    @Autowired
    public UserController(UserDatabase udb)
    {
        database = udb;
    }

    @GetMapping("/hello")
    public String hello()
    {
        database.testQuery();
        return "hello world & inserted test";
    }
}
