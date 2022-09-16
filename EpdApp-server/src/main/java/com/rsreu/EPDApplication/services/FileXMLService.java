package com.rsreu.EPDApplication.services;

import com.rsreu.EPDApplication.entities.FileXML;
import com.rsreu.EPDApplication.repositories.FileRepo;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

@Service
public class FileXMLService {

    @Autowired
    private FileRepo repo;

    public FileXML findById(Long id) {
        return repo.findById(id).orElseThrow();
    }

    public void save(FileXML file) {
        repo.save(file);
    }
}
