package com.rsreu.EPDApplication.repositories;

import com.rsreu.EPDApplication.entities.Role;
import org.springframework.data.repository.CrudRepository;

import java.util.Optional;

public interface RoleRepo extends CrudRepository<Role, Long> {
    Optional<Role> findByName(String name);
}
