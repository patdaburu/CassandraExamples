# Apache Cassandra, .Net, and Chartis

## Overview
[Apache Cassandra](<http://cassandra.apache.org/doc/latest/>) is the database platform that Airbus intends to make available to instances of the [Chartis](<https://github.com/Geo-Comm/Chartis>) desktop application.  This document discusses aspects of Cassandra as they apply to .Net in general, and the Chartis project in particular.

## Resources
- [Cassandra Documentation](<http://cassandra.apache.org/doc/latest/>)
- [Download Cassandra](<http://cassandra.apache.org/download/>)

## Installing Cassandra on Windows 10

This section describes the necessary steps to get a minimum Cassandra instance running for development purposes.  For more information, see the [Cassandra installation documentation](<http://cassandra.apache.org/doc/latest/getting_started/installing.html>).

### Prerequisites
As of this writing, the latest version of Cassandra requires [Oracle Java 8](
<http://www.oracle.com/technetwork/java/javase/downloads/index.html>) or [OpenJDK 8](<http://openjdk.java.net/>).  This document assumes you're using Oracle.

You can check the Java version using the `java -version` command.  If you have Java 8 installed, you'll see something like the following:

```sh
java version "1.8.0_111"
Java(TM) SE Runtime Environment (build 1.8.0_111-b14)
Java HotSpot(TM) 64-Bit Server VM (build 25.111-b14, mixed mode)
```

### Set `%JAVA_HOME%`
Cassandra requires that you set the `%JAVA_HOME%` environment variable.  You can figure out where `java.exe` is located by using the `where java` command.  

*Note that it is possible that this location will contain a symbolic link to `java.exe`, and that for Cassandra you will need to follow the link to the actual location.*

On a Windows 10 machine, Java may be installed at `C:\Program Files\Java\jre1.8.0_111` (or a similar location).

### Set `%JAVA_BIN%`
On Windows, Cassandra [will not start in the foreground](<http://stackoverflow.com/questions/32879568/cassandra-2-2-1-will-not-start-using-cassandra-f>) unless the `%JAVA_BIN%` environment variable is set.  As with `%JAVA_HOME%`, you can figure out where `java.exe` is located by using the `where java` command.

*Note that it is possible that this location will contain a symbolic link to `java.exe`, and that for Cassandra you will need to follow the link to the actual location.*

On a Windows 10 machine, Java may be installed at `C:\Program Files\Java\jre1.8.0_111` (or a similar location).

### PowerShell Script Execution
When you launch Cassandra, you may see the following message: *"WARNING! Powershell script execution unavailable."*  You can overcome this either by running Cassandra from a command line with Administrator privileges, or by launching `powershell` as an administrator and executing the following command:

```sh
Set-ExecutionPolicy Unrestricted
```

### Download and Extract
1. [Download Cassandra](<http://cassandra.apache.org/download/>).
2. Extract the Cassandra archive file to the location from which you intend to run it.  (This directory will hereafter be referred to as `%CASSANDRA_HOME%`.  You can create this as an environment variable if you like, but this isn't required.)
3. To make running the application a little simpler, add `%CASSANDRA_HOME%\bin` to your `%PATH%` variable.  Again, this isn't required.

### Run Cassandra
The `cassandra` executable is found in `%CASSANDRA_HOME%\bin.`  To run Cassandra in the foreground, execute the following command:

```sh
cassandra -f
```

### Verify that Cassandra is Running
You can verify that Cassandra is running using Cassandra's `nodetool` command.

```sh
nodetool status
```

After a simple installation like the one described here, you should see output similar to the following:

```sh
Datacenter: datacenter1
========================
Status=Up/Down
|/ State=Normal/Leaving/Joining/Moving
--  Address    Load       Tokens       Owns (effective)  Host ID                               Rack
UN  127.0.0.1  103.8 KiB  256          100.0%            770ca4b4-a442-427c-8860-bfa7f01fecc7  rack1
```

#Running `cqlsh`

While we will be using the .Net driver, you can also query the Cassandra database using [CQL (Cassandra Query Language)](http://cassandra.apache.org/doc/latest/cql/index.html) and [`cqlsh`](<http://cassandra.apache.org/doc/latest/tools/cqlsh.html>), the CQL shell.

##Setting the Code Page in Windows
When you launch `cqlsh` on Windows, you may see the following warning: 

```sh
*WARNING: console codepage must be set to cp65001 to support utf-8 encoding on Windows platforms.
If you experience encoding problems, change your console codepage with 'chcp 65001' before starting cqlsh.* 
```

To over come this, simply follow the instructions as provided by entering the following command in the terminal before starting `cqlsh`.

```sh
chcp 65001
cqlsh
```

#Creating a Keyspace and a Table

If you look at one of the [language driver tutorials](<https://academy.datastax.com/resources/getting-started-apache-cassandra-and-c-net>), it may recommend that you first create a keyspace and a table.

Some of the information in this section is excerpted from [Learning Apache Cassandra - Manage Fault Tolerant and Scalable Real-Time Data](<https://www.amazon.com/dp/B00U1D9WSC/ref=dp-kindle-redirect?_encoding=UTF8&btkr=1>) by Mat Brown.

##Create a Keyspace
*Keyspaces* are analogous to *databases* in a relational system in that they represent a collection of related tables.  The following command creates the *"chartis"* keyspace used in the examples.

```sh
CREATE KEYSPACE "demo"
WITH REPLICATION = {
  'class': 'SimpleStrategy', 'replication_factor': 1
};
```

Once the keyspace has been created it you tell Cassandra that all future commands will implicityly refer to tables inside the new keyspace with the `USE` statement.

```sh
USE "demo";
```

##Create a Table

```sh
CREATE TABLE "users" (
  "username" text PRIMARY KEY,
  "email" text,
  "encrypted_password" blob);
```


#The C# Driver for Apache Cassandra

There are [several C#/.Net client drivers](<http://cassandra.apache.org/doc/latest/getting_started/drivers.html>) from which to choose, but we'll start with the [DataStax C# driver](<http://datastax.github.io/csharp-driver/>) which we can install easily enough to any project using the [nuget package](<https://www.nuget.org/packages/CassandraCSharpDriver/>), which is called `CassandraCSharpDriver`.

You can use the Visual Studio's graphical NuGet package manager, or issue the following command in the package manager console:

```sh
Install-Package CassandraCSharpDriver
```

Once it's installed, you can review a [tutorial](<https://academy.datastax.com/resources/getting-started-apache-cassandra-and-c-net>) on using the C#/.Net driver.
