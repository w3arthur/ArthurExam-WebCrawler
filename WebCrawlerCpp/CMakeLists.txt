cmake_minimum_required (VERSION 3.8)

project ("WebCrawlerCpp")

set(PROJECT_MAIN_NAME WebCrawlerCpp)

add_executable (${PROJECT_MAIN_NAME} "WebCrawlerCpp.cpp")

set (CMAKE_CXX_STANDARD 20)
set (CMAKE_C_STANDARD 17)
set(CMAKE_EXE_LINKER_FLAGS -static)


    #VCPKG
#clion set for each compile option, Cmake options  -DCMAKE_TOOLCHAIN_FILE=C:/src/vcpkg/vcpkg/scripts/buildsystems/vcpkg.cmake
#set(VCPKG_INCLUDE_DIRS "C:/src/vcpkg/vcpkg/installed/x86-windows/include")
#include_directories(${VCPKG_INCLUDE_DIRS})
    # use the 64 bit version, (check why 86 not works)
find_package(CURL CONFIG REQUIRED)
target_link_libraries(${PROJECT_MAIN_NAME} PRIVATE CURL::libcurl)
find_package(nlohmann_json REQUIRED)
target_link_libraries(${PROJECT_MAIN_NAME} PRIVATE nlohmann_json::nlohmann_json)
set(nlohmann-json_IMPLICIT_CONVERSIONS OFF)


    #Boost
# please set the environments for boost
    #set Boost_INCLUDE_DIR=C:\local\boost_1_80_0
    #set Boost_INCLUDEDIR=C:\local\boost_1_80_0
    #set BOOST_LIBRARY_DIR=C:\local\boost_1_80_0\lib64-msvc-14.1
    #set Boost_LIBRARYDIR=C:\local\boost_1_80_0\lib64-msvc-14.1
    #set BOOST_ROOT=C:\local\boost_1_80_0
#set (Boost_DEBUG 1)
set (Boost_USE_STATIC_LIBS OFF) # enable dynamic linking
set (Boost_USE_MULTITHREAD ON)  # enable multithreading
find_package(Boost) #Boost 1.80.0
if (Boost_FOUND)
    include_directories(${Boost_INCLUDE_DIR})   #env
else()
    message(FATAL_ERROR "~~~ ! Boost Fail ! ~~~")
endif()


message(STATUS "~~~ CMake Starts, Compile starts ~~~")

# sub projects
# https://www.jetbrains.com/help/clion/2018.2/quick-cmake-tutorial.html#d156825e282


#example of include libs
#include_directories( ${MY_SOURCE_DIR}/src )
#include_directories( BEFORE ${MY_SOURCE_DIR}/src )
#include_directories( AFTER ${MY_SOURCE_DIR}/src )