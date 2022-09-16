package com.rsreu.EPDApplication.controllers;

import com.rsreu.EPDApplication.dto.UserDTO;
import com.rsreu.EPDApplication.entities.User;
import com.rsreu.EPDApplication.services.UserService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;

@Controller
@RequestMapping("/user")
public class UserController {

    @Autowired
    UserService service;


    @PostMapping("/auth")
    public ResponseEntity authorize(@RequestBody User user) {
        try {
            User user1 = service.findByLogin(user.getLogin());
            if (user1.getPassword().equals(user.getPassword())) return ResponseEntity.ok().body(UserDTO.toDTO(user1));
            else return ResponseEntity.badRequest().body("Неверный пароль");
        } catch (Exception e) {
            return ResponseEntity.badRequest().body(e.getStackTrace());
        }
    }

    @PostMapping
    public ResponseEntity addUser(@RequestBody User user) {
        try {
            service.save(user);
            return ResponseEntity.ok().body("User added");
        } catch (Exception e) {
            return ResponseEntity.badRequest().body(e.getMessage());
        }
    }

    @GetMapping("/{id}")
    public ResponseEntity getUser(@PathVariable Long id) {
        try {
            return ResponseEntity.ok(UserDTO.toDTO(service.findById(id)));
        } catch (Exception e) {
            return ResponseEntity.badRequest().body(e.getMessage());
        }
    }
}
