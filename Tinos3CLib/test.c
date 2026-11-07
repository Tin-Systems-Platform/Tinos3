typedef unsigned short char16_t;

extern char16_t* test()
{
    static char16_t hello[] = u"Hello from GCC";
    return hello;
}

// native/screen.c
#include <stdint.h>

int get_status_text(char* buffer, uint64_t capacity)
{
    const char* text = "C is alive";

    uint64_t i = 0;
    while (text[i] != '\0' && i + 1 < capacity)
    {
        buffer[i] = text[i];
        i++;
    }

    buffer[i] = '\0';
    return (int)i;
}