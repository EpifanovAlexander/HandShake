package com.rsreu.EPDApplication.controllers;

import com.rsreu.EPDApplication.EpdApplication;
import com.rsreu.EPDApplication.entities.FileXML;
import com.rsreu.EPDApplication.entities.MyKeyPair;
import com.rsreu.EPDApplication.repositories.KeyPairRepo;
import com.rsreu.EPDApplication.services.FileXMLService;
import org.bouncycastle.asn1.x509.X509Name;
import org.bouncycastle.crypto.AsymmetricCipherKeyPair;
import org.bouncycastle.crypto.KeyParser;
import org.bouncycastle.crypto.generators.RSAKeyPairGenerator;
import org.bouncycastle.openssl.PEMKeyPair;
import org.bouncycastle.openssl.PEMParser;
import org.bouncycastle.x509.X509V1CertificateGenerator;
import org.bouncycastle.x509.X509V3CertificateGenerator;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;

import java.io.ByteArrayInputStream;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.StringReader;
import java.math.BigInteger;
import java.nio.charset.StandardCharsets;
import java.security.KeyFactory;
import java.security.cert.X509Certificate;
import java.security.spec.PKCS8EncodedKeySpec;
import java.time.LocalDateTime;
import java.time.ZoneId;
import java.util.Date;
import java.util.Objects;

import static com.fasterxml.jackson.databind.type.LogicalType.DateTime;

@Controller
@RequestMapping("/file")
public class FileController {

    @Autowired
    private FileXMLService service;
    @Autowired
    private KeyPairRepo keyRepo;

    @GetMapping("/{id}")
    public ResponseEntity getFile(@PathVariable Long id) {
        try {
            String result = new String(service.findById(id).getFile().getBytes(), StandardCharsets.UTF_8);
            return ResponseEntity.ok().body(result);
        } catch (Exception e) {
            return ResponseEntity.badRequest().body(e.getStackTrace());
        }
    }

    @GetMapping("/init")
    public ResponseEntity init() {
        try {
            AsymmetricCipherKeyPair keyPair = new RSAKeyPairGenerator().generateKeyPair();
            keyRepo.save(MyKeyPair.builder().mKey(keyPair.toString().getBytes()).build());
            for (long i = 1; i <= 100; i++) {
                String path = "epd/ON_TRNACLGROT_" + i + ".xml";
                FileXML file = new FileXML();
                file.setId(i);
                InputStreamReader reader = new InputStreamReader(Objects.requireNonNull(EpdApplication.class.getClassLoader()
                        .getResourceAsStream(path)), "windows-1251");
                StringBuilder builder = new StringBuilder();
                while (reader.ready()) {
                    builder.append((char) reader.read());
                }
                file.setFile(builder.toString());
                service.save(file);
            }
            return ResponseEntity.ok().body("ok");
        } catch (Exception e) {
            return ResponseEntity.badRequest().body(e.toString());
        }
    }

    @PostMapping("/{id}")
    public ResponseEntity uploadFile(@PathVariable Long id, @RequestBody String text) {
        try {
            FileXML xml = FileXML.builder().id(id).file(text).build();
            service.save(xml);
            return ResponseEntity.ok().body("File uploaded");
        } catch (Exception e) {
            return ResponseEntity.badRequest().body(e.getMessage());
        }
    }

    @GetMapping("/crt/{id}")
    public ResponseEntity getCertificate(@PathVariable Long id) {
        try {
            PEMParser parser = new PEMParser(new StringReader(new String(keyRepo.findById(1L).orElseThrow().getMKey())));
            PEMKeyPair keyPair = (PEMKeyPair) parser.readObject();
            byte[] encoder = keyPair.getPrivateKeyInfo().getEncoded();
            KeyFactory kf = KeyFactory.getInstance("RSA");
            X509V3CertificateGenerator generator = new X509V3CertificateGenerator();
            generator.setSerialNumber(BigInteger.valueOf(id));
            generator.setSubjectDN(new X509Name("CN=CA"));
            generator.setIssuerDN(new X509Name("CN=CA"));
            generator.setNotAfter(Date.from(LocalDateTime.now().plusYears(100L).atZone(ZoneId.systemDefault()).toInstant()));
            generator.setNotBefore(new Date());
            generator.setSignatureAlgorithm("SHA1WITHRSA");
            generator.setPublicKey(kf.generatePublic(new PKCS8EncodedKeySpec(encoder)));
            X509Certificate cert = generator.generate(kf.generatePrivate(new PKCS8EncodedKeySpec(encoder)));
            return ResponseEntity.ok(cert.toString());
        } catch (Exception e) {
            return ResponseEntity.badRequest().body(e.toString());
        }
    }
}