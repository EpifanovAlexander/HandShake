package com.rsreu.EPDApplication.controllers;

import com.rsreu.EPDApplication.entities.Role;
import com.rsreu.EPDApplication.repositories.RoleRepo;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;

@Controller
@RequestMapping("/role")
public class RoleController {

    @Autowired
    RoleRepo repo;

    @PostMapping
    public ResponseEntity addRole(@RequestBody Role role) {
        try {
            repo.save(role);
            return ResponseEntity.ok("Role added");
        } catch (Exception e) {
            return ResponseEntity.badRequest().body(e.getMessage());
        }
    }
}
