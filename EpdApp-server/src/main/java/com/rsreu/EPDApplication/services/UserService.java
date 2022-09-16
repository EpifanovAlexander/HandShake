package com.rsreu.EPDApplication.services;

import com.rsreu.EPDApplication.dto.UserDTO;
import com.rsreu.EPDApplication.entities.User;
import com.rsreu.EPDApplication.repositories.RoleRepo;
import com.rsreu.EPDApplication.repositories.UserRepo;
import lombok.AllArgsConstructor;
import lombok.NoArgsConstructor;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

@Service
@AllArgsConstructor
@NoArgsConstructor
public class UserService {

    @Autowired
    UserRepo repo;
    @Autowired
    RoleRepo roleRepo;

    public void save(User user) {
        repo.save(user);
    }

    public User findById(Long id) {
        return repo.findById(id).orElseThrow();
    }

    public User findByLogin(String login) {
        return repo.findByLogin(login);
    }

    public void delete(Long id) {
        repo.findById(id).ifPresent(value -> repo.delete(value));
    }

    public void update(UserDTO user, Long id) {
        User user1 = repo.findById(id).orElseThrow();
        user1.setName(user.getName());
        user1.setLogin(user.getLogin());
        user1.setRole(roleRepo.findByName(user.getRole()).orElseThrow());
    }
}
