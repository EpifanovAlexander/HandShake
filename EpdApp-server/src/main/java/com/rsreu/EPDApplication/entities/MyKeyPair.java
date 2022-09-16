package com.rsreu.EPDApplication.entities;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.Table;

@Entity
@Data
@NoArgsConstructor
@AllArgsConstructor
@Builder
@Table(name = "keys")
public class MyKeyPair {

    @Id
    private Long id;
    @Column(name = "keypair")
    private byte[] mKey;
}
