# LogAssignment

## Design and architecture 
(SVG version is included in the project)

![LogComponent](https://user-images.githubusercontent.com/64922954/228685130-3c65e826-3156-44c3-ad3d-6b8f63a5da2f.png)

## Unit tests

The unit tests has been developed using xUnit framework and are available under the LogTests project.
To run the tests please execute the LogTest.cs

There are currently 4 .log files generated in the LogAssignment\LogResults dir representing the result from running the unit tests.
![image](https://user-images.githubusercontent.com/64922954/228686218-85808b75-3f40-417e-9cb5-9860965ef196.png)

The first two log files are generated demonstrating the creation of a new log file after midnight. The other two files represent, one with numbers going from 50 down to something – when the component is stopped without flush. One file having logs with numbers going from 0 to 14.

The unit tests are missing a method cleaning up the log directory before executing the tests again. Unit tests for handling the robustness of the system against failures and exceptions have not been provided either.
