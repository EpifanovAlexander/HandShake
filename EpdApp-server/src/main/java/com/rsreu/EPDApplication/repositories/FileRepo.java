package com.rsreu.EPDApplication.repositories;

import com.rsreu.EPDApplication.entities.FileXML;
import org.springframework.data.repository.CrudRepository;
import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public interface FileRepo extends CrudRepository<FileXML, Long> {
}
