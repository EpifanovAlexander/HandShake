package com.rsreu.EPDApplication.repositories;

import com.rsreu.EPDApplication.entities.User;
import org.springframework.data.repository.CrudRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface UserRepo extends CrudRepository<User, Long> {
    User findByLogin(String login);
}
