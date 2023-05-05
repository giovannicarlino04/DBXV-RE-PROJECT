import os
from CMS import CMS
import configparser
import struct

config = configparser.ConfigParser()
config.read('Xenoverse.ini')

data_path = config['DATA']['path']
cms_path = config['CMS']['path']

def print_menu():
    print("1. Add a new character")
    print("2. Edit Charalist")
    print("3. Compile Iggy Scripts")

def shift_header_offset(filename):
    # read the file and get its size
    with open(filename, 'rb') as f:
        file_data = f.read()
    file_size = os.path.getsize(filename)

    # update the offset value in the header
    offset = struct.unpack('I', file_data[0x08:0x0C])[0]
    new_offset = offset + 1
    new_offset_bytes = struct.pack('I', new_offset)
    new_file_data = file_data[:0x08] + new_offset_bytes + file_data[0x0C:]

    # write the updated file data back to the file
    with open(filename, 'wb') as f:
        f.write(new_file_data)

def compile_iggy():
    os.system('cd "' + os.getcwd() + '" && mxmlc -compiler.source-path=.\scripts .\scripts\dlc3_CHARASELE_fla\MainTimeline.as && move /Y .\scripts\dlc3_CHARASELE_fla\MainTimeline.swf "' + data_path + '\\ui\\iggy\\CHARASELE.swf" && cd ' + data_path + '/ui/iggy && iggy_as3_test.exe CHARASELE.swf')

def edit_charalist():
    os.system(' "' + os.getcwd() + '\\scripts\\action_script\\CharaList.as"')

def add_character(filename):
    # read the file and get its size
    with open(filename, 'rb') as f:
        file_data = f.read()
    file_size = os.path.getsize(filename)

    # define the new character data
    character_id = int(input("Insert character number ID: "))
    character_name = input("Insert character name: ")
    name_length = len(character_name)
    character_name_bytes = character_name.encode('utf-8')
    unknown_data = b'\x00\x00\x00\x00' + b'\x00' * 9 + b'\x01\x00\x00\x00' + b'\xff\xff\x07\x00' + b'\x00' * 16
    new_character_data = character_id.to_bytes(1, byteorder='little') + b'\x00' * 3 + character_name_bytes + b'\x00' * (32 - name_length) + unknown_data + b'\x00' * 7 

    # calculate the offset to insert the new character data
    last_character_offset = 0x1D20
    character_size = 0x50
    new_character_offset = last_character_offset + character_size
    new_file_data = file_data[:new_character_offset] + new_character_data + file_data[new_character_offset:]

    # write the updated file data back to the file
    with open(filename, 'wb') as f:
        f.write(new_file_data)

def shift_addresses(offsets, filename, character_size):
    # calculate the byte offset of each address for every character block
    char_offsets = [(i*80 + offset) for i in range(94) for offset in offsets]

    with open(filename, 'r+b') as f:
        # shift each address in every character block
        for address_offset in char_offsets:
            f.seek(address_offset)
            new_pointer_value = int.from_bytes(f.read(4), byteorder='little') + character_size
            f.seek(address_offset)
            f.write(new_pointer_value.to_bytes(4, byteorder='little'))
            
    with open(filename, 'rb') as f:
        file_data = f.read()

    # print the old and new values of each shifted address
    for address_offset in char_offsets:
        pointer_value = int.from_bytes(file_data[address_offset:address_offset+4], byteorder='little')
        new_pointer_value = int.from_bytes(file_data[address_offset:address_offset+4], byteorder='little') + character_size
        #print(f'Address offset: {hex(address_offset)}, old value: {hex(pointer_value)}, new value: {hex(new_pointer_value)}')

    with open(filename, 'wb') as f:
        f.write(file_data)

def main():
    print("Welcome to XenoPy!")
    if not os.path.isfile(cms_path):
        print(f"File {cms_path} not found!")
        return
    print_menu()
    choice = input("Enter your choice: ")

    if choice == "1":
        add_character(cms_path)
        # List of offsets for each specific address within each character block
        shift_header_offset(cms_path)

        compile_iggy()
        print("New character added successfully!")
    elif choice == "2":
        edit_charalist()
    elif choice == "3":
        compile_iggy()
    else:
        print(f"Invalid choice {choice}!")

if __name__ == "__main__":
    main()