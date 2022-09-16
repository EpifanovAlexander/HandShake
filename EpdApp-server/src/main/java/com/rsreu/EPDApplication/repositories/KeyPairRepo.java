package com.rsreu.EPDApplication.repositories;

import com.rsreu.EPDApplication.entities.MyKeyPair;
import org.springframework.data.repository.CrudRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface KeyPairRepo extends CrudRepository<MyKeyPair, Long> {
}
