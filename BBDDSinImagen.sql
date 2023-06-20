CREATE DATABASE  IF NOT EXISTS `gestion_incidencias` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `gestion_incidencias`;
-- MySQL dump 10.13  Distrib 8.0.26, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: gestion_incidencias
-- ------------------------------------------------------
-- Server version	5.7.38-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `departamento`
--

DROP TABLE IF EXISTS `departamento`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `departamento` (
  `codigo_dep` int(11) NOT NULL,
  `nombre_dep` varchar(45) NOT NULL,
  PRIMARY KEY (`codigo_dep`),
  UNIQUE KEY `codigo_dep_UNIQUE` (`codigo_dep`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `departamento`
--

LOCK TABLES `departamento` WRITE;
/*!40000 ALTER TABLE `departamento` DISABLE KEYS */;
INSERT INTO `departamento` VALUES (1,'Informática'),(2,'Castellano'),(3,'Arte y música'),(4,'Física y química'),(5,'Matemáticas'),(6,'Educación física'),(7,'Inglés'),(8,'Historia');
/*!40000 ALTER TABLE `departamento` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `estados`
--

DROP TABLE IF EXISTS `estados`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `estados` (
  `codigo_estado` int(11) NOT NULL,
  `desc_estado` varchar(45) NOT NULL,
  PRIMARY KEY (`codigo_estado`),
  UNIQUE KEY `codigo_estado_UNIQUE` (`codigo_estado`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `estados`
--

LOCK TABLES `estados` WRITE;
/*!40000 ALTER TABLE `estados` DISABLE KEYS */;
INSERT INTO `estados` VALUES (1,'Comunicada'),(2,'En solución'),(3,'Solucionada'),(4,'Solución inviable');
/*!40000 ALTER TABLE `estados` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `hardware`
--

DROP TABLE IF EXISTS `hardware`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `hardware` (
  `id_incidencia_hw` int(11) NOT NULL AUTO_INCREMENT,
  `num_serie` int(11) NOT NULL,
  `modelo` varchar(45) DEFAULT NULL,
  `tipo_hardware_id` int(11) NOT NULL,
  PRIMARY KEY (`id_incidencia_hw`),
  UNIQUE KEY `id_incidencia_hw_UNIQUE` (`id_incidencia_hw`),
  KEY `fk_hardware_tipo_hardware1_idx` (`tipo_hardware_id`),
  CONSTRAINT `fk_hardware_tipo_hardware1` FOREIGN KEY (`tipo_hardware_id`) REFERENCES `tipo_hardware` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=36 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `hardware`
--

LOCK TABLES `hardware` WRITE;
/*!40000 ALTER TABLE `hardware` DISABLE KEYS */;
INSERT INTO `hardware` VALUES (29,62345,'fdas',2),(30,523411,'Server',2),(33,452344234,'Monitor',3),(34,4535345,'Speaker',5),(35,2331,'Proyector',2);
/*!40000 ALTER TABLE `hardware` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `incidencia`
--

DROP TABLE IF EXISTS `incidencia`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `incidencia` (
  `num_incidencia` int(11) NOT NULL AUTO_INCREMENT,
  `tipo_incidencia` varchar(45) NOT NULL,
  `fecha_incidencia` date NOT NULL,
  `fecha_introduccion` date NOT NULL,
  `aula_ubicacion` varchar(45) NOT NULL,
  `descripcion_incidencia` varchar(100) NOT NULL,
  `observaciones` varchar(100) DEFAULT NULL,
  `fecha_resolución` date DEFAULT NULL,
  `estados_codigo_estado` int(11) NOT NULL,
  `departamento_codigo_dep` int(11) NOT NULL,
  `profesor_dni` varchar(10) NOT NULL,
  `tiempo_invertido` double DEFAULT NULL,
  `info_acompanyada` longblob,
  `hardware_id_incidencia_hw` int(11) DEFAULT NULL,
  `dni_responsable` varchar(10) NOT NULL,
  PRIMARY KEY (`num_incidencia`),
  UNIQUE KEY `num_incidencia_UNIQUE` (`num_incidencia`),
  KEY `fk_incidencia_estados1_idx` (`estados_codigo_estado`),
  KEY `fk_incidencia_departamento1_idx` (`departamento_codigo_dep`),
  KEY `fk_incidencia_profesor1_idx` (`profesor_dni`),
  KEY `fk_incidencia_hardware1_idx` (`hardware_id_incidencia_hw`),
  KEY `fk_incidencia_profesor2_idx` (`dni_responsable`),
  CONSTRAINT `fk_incidencia_departamento1` FOREIGN KEY (`departamento_codigo_dep`) REFERENCES `departamento` (`codigo_dep`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_incidencia_estados1` FOREIGN KEY (`estados_codigo_estado`) REFERENCES `estados` (`codigo_estado`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_incidencia_hardware1` FOREIGN KEY (`hardware_id_incidencia_hw`) REFERENCES `hardware` (`id_incidencia_hw`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_incidencia_profesor1` FOREIGN KEY (`profesor_dni`) REFERENCES `profesor` (`dni`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_incidencia_profesor2` FOREIGN KEY (`dni_responsable`) REFERENCES `profesor` (`dni`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=56 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `incidencia`
--

LOCK TABLES `incidencia` WRITE;
/*!40000 ALTER TABLE `incidencia` DISABLE KEYS */;
INSERT INTO `incidencia` VALUES (3,'Software','2022-07-08','2023-04-16','asdasd','asdasd','',NULL,1,2,'1234',NULL,NULL,NULL,'876543210A'),(7,'Software','2023-01-05','2023-04-28','Aula 24','Se congela las aplicaciones y luego se cierran','',NULL,1,7,'1234',NULL,NULL,NULL,'1234'),(10,'Software','2022-11-12','2023-04-29','Aula 103','Se cierra MySQL Workbench','',NULL,1,1,'1234',NULL,NULL,NULL,'1234'),(18,'Software','2023-04-30','2023-04-30','asd','asd','',NULL,1,2,'1234',NULL,NULL,NULL,'1234'),(19,'Hardware','2023-04-30','2023-04-30','sdf','sdf','',NULL,1,6,'1234',NULL,NULL,29,'1234'),(20,'Hardware','2022-09-10','2023-04-30','Aula 205','Se quemó el servidor','',NULL,1,5,'1234',NULL,NULL,30,'1234'),(21,'Hardware','2023-07-13','2023-04-30','Aula 201','Se rompió el monitor','',NULL,1,5,'1234',NULL,NULL,33,'1234'),(22,'Hardware','2023-02-10','2023-05-03','Aula 205','Se ha caído y se rompió','',NULL,1,4,'1234',NULL,NULL,34,'1234'),(23,'Hardware','2023-05-03','2023-05-03','Aula 304','Se rompió','',NULL,1,2,'1234',NULL,NULL,35,'1234'),(29,'Software','2023-05-11','2023-05-11','sdfsdf','sdfsdf','',NULL,1,2,'1234',NULL,NULL,NULL,'4321'),(30,'Software','2023-05-11','2023-05-11','dssdf','sdfsdf','',NULL,1,1,'1234',NULL,NULL,NULL,'1234'),(31,'Software','2023-05-11','2023-05-11','fsadfsdf','412344','',NULL,2,1,'1234',NULL,NULL,NULL,'4321'),(32,'Software','2023-05-11','2023-05-11','Aula 393','Se crashea todo','',NULL,2,6,'1234',NULL,NULL,NULL,'4321'),(33,'Software','2023-05-11','2023-05-11','gfdsgsdfg','sdfgdfg','',NULL,1,1,'1234',NULL,NULL,NULL,'4321'),(34,'Software','2023-05-11','2023-05-11','asd','asd','sdfsdff','2023-05-11',3,5,'1234',12,NULL,NULL,'4321'),(35,'Software','2023-05-11','2023-05-11','hjgjhgh','ghjjghghj','','2023-05-15',2,3,'1234',NULL,NULL,NULL,'4321'),(36,'Software','2023-05-11','2023-05-11','gfdsgsdg','sdgsdgfsgf','',NULL,1,1,'1234',NULL,NULL,NULL,'4321'),(37,'Software','2023-05-11','2023-05-11','fgh','fgh','',NULL,1,2,'1234',NULL,NULL,NULL,'4321'),(38,'Software','2023-05-16','2023-05-16','gfds','sdfg','',NULL,1,3,'1234',NULL,NULL,NULL,'4321'),(49,'Software','2023-05-18','2023-05-18','fsdf','sdfsdf','',NULL,1,3,'1234',NULL,NULL,NULL,'876543210A'),(50,'Software','2023-05-18','2023-05-18','dfsf','sdfsdf','',NULL,1,2,'1234',NULL,NULL,NULL,'4321'),(51,'Software','2023-05-18','2023-05-18','sdfsdf','sdfsdf','',NULL,1,3,'1234',NULL,NULL,NULL,'876543210A'),(52,'Software','2023-05-18','2023-05-18','sfd','sdf','',NULL,1,3,'1234',NULL,NULL,NULL,'4321'),(53,'Software','2023-05-18','2023-05-18','gdfgfd','dfgdfg','',NULL,1,7,'1234',NULL,NULL,NULL,'876543210A'),(55,'Software','2023-06-06','2023-06-06','sdfsdf','sdfsdf','',NULL,1,3,'1234',23.4,NULL,NULL,'33662234');
/*!40000 ALTER TABLE `incidencia` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `permisos`
--

DROP TABLE IF EXISTS `permisos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `permisos` (
  `cod_permiso` int(11) NOT NULL AUTO_INCREMENT,
  `desc_permiso` varchar(45) NOT NULL,
  PRIMARY KEY (`cod_permiso`),
  UNIQUE KEY `cod_permiso_UNIQUE` (`cod_permiso`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `permisos`
--

LOCK TABLES `permisos` WRITE;
/*!40000 ALTER TABLE `permisos` DISABLE KEYS */;
INSERT INTO `permisos` VALUES (1,'Añadir incidencia'),(2,'Modificar/Borrar incidencias'),(3,'Añadir,borrar o modificar tipos de Hardware'),(4,'Alta/baja/modificación de roles y permisos'),(5,'Operaciones de importación/exportación'),(6,'Informes sobre incidencias');
/*!40000 ALTER TABLE `permisos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `profesor`
--

DROP TABLE IF EXISTS `profesor`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `profesor` (
  `dni` varchar(10) NOT NULL,
  `nombre` varchar(50) NOT NULL,
  `apellidos` varchar(50) NOT NULL,
  `email` varchar(45) NOT NULL,
  `contraseña` varchar(30) DEFAULT NULL,
  `departamento_codigo_dep` int(11) NOT NULL,
  `roles_rol` varchar(45) NOT NULL,
  PRIMARY KEY (`dni`),
  UNIQUE KEY `dni_UNIQUE` (`dni`),
  KEY `fk_profesor_departamento1_idx` (`departamento_codigo_dep`),
  KEY `fk_profesor_roles1_idx` (`roles_rol`),
  CONSTRAINT `fk_profesor_departamento1` FOREIGN KEY (`departamento_codigo_dep`) REFERENCES `departamento` (`codigo_dep`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_profesor_roles1` FOREIGN KEY (`roles_rol`) REFERENCES `roles` (`rol`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `profesor`
--

LOCK TABLES `profesor` WRITE;
/*!40000 ALTER TABLE `profesor` DISABLE KEYS */;
INSERT INTO `profesor` VALUES ('123','ab','ab','ab@email.com','123',5,'Directivo'),('1234','a','b','v','1234',1,'Administrador'),('33662234','Profesor','Demo','profesor@demo.com','1234',2,'Coordinador TIC'),('4321','Juan','Juanez','juan@email.com','4321',4,'Directivo'),('876543210A','Jorge','Campo','jukddlskjf@gmail.com','hola',6,'Profesor');
/*!40000 ALTER TABLE `profesor` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `roles`
--

DROP TABLE IF EXISTS `roles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `roles` (
  `rol` varchar(45) NOT NULL DEFAULT 'profesor',
  PRIMARY KEY (`rol`),
  UNIQUE KEY `rol_UNIQUE` (`rol`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `roles`
--

LOCK TABLES `roles` WRITE;
/*!40000 ALTER TABLE `roles` DISABLE KEYS */;
INSERT INTO `roles` VALUES ('Administrador'),('Coordinador TIC'),('Directivo'),('Profesor');
/*!40000 ALTER TABLE `roles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `roles_has_permisos`
--

DROP TABLE IF EXISTS `roles_has_permisos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `roles_has_permisos` (
  `roles_rol` varchar(45) NOT NULL,
  `permisos_cod_permiso` int(11) NOT NULL,
  PRIMARY KEY (`roles_rol`,`permisos_cod_permiso`),
  KEY `fk_roles_has_permisos_permisos1_idx` (`permisos_cod_permiso`),
  KEY `fk_roles_has_permisos_roles1_idx` (`roles_rol`),
  CONSTRAINT `fk_roles_has_permisos_permisos1` FOREIGN KEY (`permisos_cod_permiso`) REFERENCES `permisos` (`cod_permiso`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_roles_has_permisos_roles1` FOREIGN KEY (`roles_rol`) REFERENCES `roles` (`rol`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `roles_has_permisos`
--

LOCK TABLES `roles_has_permisos` WRITE;
/*!40000 ALTER TABLE `roles_has_permisos` DISABLE KEYS */;
INSERT INTO `roles_has_permisos` VALUES ('Administrador',1),('Coordinador TIC',1),('Directivo',1),('Profesor',1),('Administrador',2),('Coordinador TIC',2),('Directivo',2),('Administrador',3),('Coordinador TIC',3),('Directivo',3),('Administrador',4),('Coordinador TIC',4),('Directivo',4),('Administrador',5),('Administrador',6),('Coordinador TIC',6),('Directivo',6),('Profesor',6);
/*!40000 ALTER TABLE `roles_has_permisos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tipo_hardware`
--

DROP TABLE IF EXISTS `tipo_hardware`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tipo_hardware` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `nombre` varchar(100) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tipo_hardware`
--

LOCK TABLES `tipo_hardware` WRITE;
/*!40000 ALTER TABLE `tipo_hardware` DISABLE KEYS */;
INSERT INTO `tipo_hardware` VALUES (1,'Ordenador'),(2,'Servidor'),(3,'Monitor'),(5,'Altavoces'),(6,'Router'),(7,'Micrófono'),(8,'Periféricos'),(9,'Monitor');
/*!40000 ALTER TABLE `tipo_hardware` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2023-06-06 11:52:33
