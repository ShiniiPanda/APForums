<!-- PROJECT SHIELDS -->
<!--
*** I'm using markdown "reference style" links for readability.
*** Reference links are enclosed in brackets [ ] instead of parentheses ( ).
*** See the bottom of this document for the declaration of the reference variables
*** for contributors-url, forks-url, etc. This is an optional, concise syntax you may use.
*** https://www.markdownguide.org/basic-syntax/#reference-style-links
-->
<!-- PROJECT LOGO -->
<br />
<div align="center">
  <a href="https://github.com/ShiniiPanda/APForums">
    <img src="resources/banner.png" alt="Logo" width="800" height="200">
  </a>

<h3 align="center">APForums</h3>

  <p align="center">
    A Cross-Platform Social Networking Application for <a href="https://www.apu.edu.my/">Asia Pacific University (MY)</a> Students.
    <br />
    <a href="https://github.com/ShiniiPanda/APForums/issues" style="color: purple;">Report Bug</a>
    ·
    <a href="https://github.com/ShiniiPanda/APForums/issues" style="color: purple;">Request Feature</a>
  </p>
</div>



<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#backend-instructions">Backend Instructions</a></li>
        <li><a href="#frontend-instructions">Frontend Instructions</a></li>
      </ul>
    </li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
    <li><a href="#acknowledgments">Acknowledgments</a></li>
  </ol>
</details>



<!-- ABOUT THE PROJECT -->
## About The Project

[![Product Name Screen Shot][product-screenshot]](https://example.com)

APForums came about from the wish to ameliorate interpersonal communication among the student body at Asia Pacific University.
The project was developed by <a href="https://www.github.com/ShiniiPanda">Abdelrahman Ashraf</a> as part of their final year assessment at the university.
The platform was built to support native functionality across a number of platforms (Windows, Android, iOS, MacOS) to allow for ease of accessibility to a wider audience.

<p align="right">(<a href="#readme-top">back to top</a>)</p>



### Built With

* [![.NET][.NET]][.NET-url]
* [![Blazor][Blazor]][Blazor-url]
* [![MAUI][MAUI]][MAUI-url]
* [![Tailwind CSS][TailwindCSS]][TailwindCSS-url]
* [![Maria DB][MariaDB]][MariaDB-url]

The backend was built using ASP.NET API, EntityFrameworkCore 7, and MariaDB.


<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- GETTING STARTED -->
## Getting Started

There are two steps to running this application. 
Firstly, you need to start up the server. You may follow the steps <a href="#backend-instructions">here</a>.
Having started the server, you may either compile the application or run the compiled version. You may following the steps <a href="#frontend-instructions">here</a>.


### Prerequisites

Before anything, there are some pre-requisites that you need to consider before running this application.
The front-end of the application supports multiple platforms and is built using Microsoft's new .NET MAUI framework.
For each target operating system, there are some prerequisites that should be meet to ensure stable execution:

* Android: Android 7.0 (API 24) or higher is required.
* iOS: iOS 14 or higher is required.
* macOS: macOS 11 or higher, using Mac Catalyst.
* Windows: Windows 11 and Windows 10 version 1809 or higher, using Windows UI Library (WinUI) 3.

The application is packaged with the required .net classes and libraries.
However, if you intend to run the server or compile anything from source, please make sure you have .NET 7 installed on your system with access to the dotnet cli.


- 

### Backend Instructions

1. Clone the repository
2. cd into APForums.Server
   ```sh
   cd APForums.Server
   ```
3. Open a terminal window in the current directory (APForums.Server).
   
4. Start the server by running the following command
   ```sh
   dotnet run --launch-profile http
   ```

<p align="right">(<a href="#readme-top">back to top</a>)</p>



### Frontend Instructions

1. Clone the repository
2. cd into APForums.Client
   ```sh
   cd APForums.Client
   ```
3. Access latest compiled source in /bin/Debug/[target-framework]
   Or compile from source by opening the project in Visual Studio
   (CLI tools for MAUI are still being worked on by the Microsoft team).

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- USAGE EXAMPLES -->
## Usage

Use this space to show useful examples of how a project can be used. Additional screenshots, code examples and demos work well in this space. You may also link to more resources.

<p align="right">(<a href="#readme-top">back to top</a>)</p>


<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- CONTRIBUTING -->
## Contributing

Contributions are what make the open source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

If you have a suggestion that would make this better, please fork the repo and create a pull request. You can also simply open an issue with the tag "enhancement".
Don't forget to give the project a star! Thanks again!

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- LICENSE -->
## License

Distributed under the MIT License.

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- CONTACT -->
## Contact

Abdelrahman Ashraf - [abdelrahman-ashraf-ahmed](https://www.linkedin.com/in/abdelrahman-ashraf-ahmed/) - abdelrahman.as.ah@gmail.com

Project Link: [https://github.com/ShiniiPanda/APForums](https://github.com/ShiniiPanda/APForums)

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- ACKNOWLEDGMENTS -->
## Acknowledgments

* [My wonderful FYP supervisor: Mr. Dhason Padmakumar @ Asia Pacific University](https://www.linkedin.com/in/padmakumar-dhason-02002129/)
* My friends and family!

<p align="right">(<a href="#readme-top">back to top</a>)</p>


<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[contributors-shield]: https://img.shields.io/github/contributors/github_username/repo_name.svg?style=for-the-badge
[contributors-url]: https://github.com/github_username/repo_name/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/github_username/repo_name.svg?style=for-the-badge
[forks-url]: https://github.com/github_username/repo_name/network/members
[stars-shield]: https://img.shields.io/github/stars/github_username/repo_name.svg?style=for-the-badge
[stars-url]: https://github.com/github_username/repo_name/stargazers
[issues-shield]: https://img.shields.io/github/issues/github_username/repo_name.svg?style=for-the-badge
[issues-url]: https://github.com/github_username/repo_name/issues
[license-shield]: https://img.shields.io/github/license/github_username/repo_name.svg?style=for-the-badge
[license-url]: https://github.com/github_username/repo_name/blob/master/LICENSE.txt
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://linkedin.com/in/linkedin_username
[product-screenshot]: resources/logo.png
[.NET]: https://img.shields.io/badge/.NET%207-passing?style=for-the-badge&logo=dotnet&logoColor=ffffff&logoSize=auto&labelColor=%23512BD4&color=000000
[.NET-url]: https://learn.microsoft.com/en-us/dotnet/
[MariaDB]: https://img.shields.io/badge/Maria%20DB-passing?style=for-the-badge&logo=mariadb&logoColor=003545&labelColor=orange&color=black
[MariaDB-url]: https://mariadb.org/
[TailwindCSS]: https://img.shields.io/badge/Tailwind%20CSS-passing?style=for-the-badge&logo=tailwindcss&logoColor=%2306B6D4&logoSize=auto&labelColor=black&color=white
[TailwindCSS-url]: https://tailwindcss.com/
[MAUI]: https://img.shields.io/badge/.NET%20MAUI-passing?style=for-the-badge&logo=dotnet&logoColor=%23512BD4&logoSize=auto&label=MAUI&labelColor=black&color=white
[MAUI-url]: https://dotnet.microsoft.com/en-us/apps/maui
[Blazor]: https://img.shields.io/badge/Blazor%20Hybrid-passing?style=for-the-badge&logo=blazor&logoColor=%23512BD4&logoSize=auto&label=Blazor&labelColor=black&color=purple
[Blazor-url]: https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor