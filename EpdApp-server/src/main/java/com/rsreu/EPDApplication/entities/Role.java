package com.rsreu.EPDApplication.entities;

import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

import javax.persistence.*;
import java.util.List;

@Entity
@Table(name = "roles")
@Data
@NoArgsConstructor
@AllArgsConstructor
public class Role {

    @Id @GeneratedValue(strategy = GenerationType.IDENTITY) @Column
    private long id;
    @Column
    private String name;
    @OneToMany(cascade = CascadeType.ALL, mappedBy = "role")
    private List<User> users;
}
