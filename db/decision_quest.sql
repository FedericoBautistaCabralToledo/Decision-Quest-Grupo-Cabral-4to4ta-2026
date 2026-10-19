-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 19-10-2026 a las 23:28:32
-- Versión del servidor: 10.4.32-MariaDB
-- Versión de PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `decision_quest`
--
CREATE DATABASE IF NOT EXISTS `decision_quest` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
USE `decision_quest`;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `batalla`
--

DROP TABLE IF EXISTS `batalla`;
CREATE TABLE `batalla` (
  `id_batalla` int(11) NOT NULL,
  `nombre_batalla` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `decision`
--

DROP TABLE IF EXISTS `decision`;
CREATE TABLE `decision` (
  `decision_id` int(11) NOT NULL,
  `nombre` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `desenlace_partida`
--

DROP TABLE IF EXISTS `desenlace_partida`;
CREATE TABLE `desenlace_partida` (
  `dialogo` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `final`
--

DROP TABLE IF EXISTS `final`;
CREATE TABLE `final` (
  `id_final` int(11) NOT NULL,
  `escena` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `logro`
--

DROP TABLE IF EXISTS `logro`;
CREATE TABLE `logro` (
  `logro_id` int(11) NOT NULL,
  `fecha_obtener` varchar(50) DEFAULT NULL,
  `nombre_logro` varchar(50) DEFAULT NULL,
  `descripcion` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `nodo`
--

DROP TABLE IF EXISTS `nodo`;
CREATE TABLE `nodo` (
  `id_nodo` int(11) NOT NULL,
  `nombre_nodo` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `partida`
--

DROP TABLE IF EXISTS `partida`;
CREATE TABLE `partida` (
  `id_partida` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `registrar`
--

DROP TABLE IF EXISTS `registrar`;
CREATE TABLE `registrar` (
  `id_derrota` int(11) DEFAULT NULL,
  `id_victoria` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuario`
--

DROP TABLE IF EXISTS `usuario`;
CREATE TABLE `usuario` (
  `id_usuario` int(11) NOT NULL,
  `nombre_usuario` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `batalla`
--
ALTER TABLE `batalla`
  ADD PRIMARY KEY (`id_batalla`);

--
-- Indices de la tabla `decision`
--
ALTER TABLE `decision`
  ADD PRIMARY KEY (`decision_id`);

--
-- Indices de la tabla `final`
--
ALTER TABLE `final`
  ADD PRIMARY KEY (`id_final`);

--
-- Indices de la tabla `logro`
--
ALTER TABLE `logro`
  ADD PRIMARY KEY (`logro_id`);

--
-- Indices de la tabla `nodo`
--
ALTER TABLE `nodo`
  ADD PRIMARY KEY (`id_nodo`);

--
-- Indices de la tabla `partida`
--
ALTER TABLE `partida`
  ADD PRIMARY KEY (`id_partida`);

--
-- Indices de la tabla `usuario`
--
ALTER TABLE `usuario`
  ADD PRIMARY KEY (`id_usuario`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `batalla`
--
ALTER TABLE `batalla`
  MODIFY `id_batalla` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `decision`
--
ALTER TABLE `decision`
  MODIFY `decision_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `final`
--
ALTER TABLE `final`
  MODIFY `id_final` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `logro`
--
ALTER TABLE `logro`
  MODIFY `logro_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `nodo`
--
ALTER TABLE `nodo`
  MODIFY `id_nodo` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `partida`
--
ALTER TABLE `partida`
  MODIFY `id_partida` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `usuario`
--
ALTER TABLE `usuario`
  MODIFY `id_usuario` int(11) NOT NULL AUTO_INCREMENT;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
