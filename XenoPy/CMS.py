import os
import struct

class CMS:

    TABLE_ENTRY_SIZE = 16
    FILE_HEADER_SIZE = 32

    def get_character_offset(index):
        return 0x20 + int.from_bytes(index, byteorder='little') * 0x30

    @staticmethod
    def write_string(content, offset, value):
        padding_size = offset + len(value) - len(content)
        if padding_size > 0:
            content = CMS.add_padding(content, padding_size)
        content[offset:offset+len(value)] = value.encode()

    @staticmethod
    def add_padding(content, padding_size):
        padding = b'\x00' * padding_size
        content += padding
        return content

    @staticmethod
    def get_next_character_id(file_path):
        with open(file_path, 'rb') as f:
            binary_data = f.read()
            table_offset = CMS.get_table_offset(binary_data)
            last_id_offset = table_offset + 0x1C
            last_id = struct.unpack("<i", binary_data[last_id_offset:last_id_offset+4])[0]
            next_id = last_id + 1
            return next_id

    @staticmethod
    def write_int32(content, offset, value):
        padding_size = offset + 4 - len(content)
        if padding_size > 0:
            content = CMS.add_padding(content, padding_size)
        struct.pack_into("<i", content, offset, value)

    @staticmethod
    def get_table_offset(content):
        data = content.encode('utf-8') if isinstance(content, str) else content
        table_offset_offset = struct.unpack("<i", data[0x10:0x14])[0]
        return table_offset_offset
    
    @staticmethod
    def get_num_characters(file_path):
        file_size = os.path.getsize(file_path)
        last_char_offset = 0x1C30  # the offset of the last character
        char_size = 0x140  # size of each character entry
        num_characters = (last_char_offset - 0x10) // char_size
        return num_characters

    @staticmethod
    def get_new_character_data(character_id, character_name):
        # Create the byte sequence for one character
        characterid = character_id
        charactername = character_name
        name_bytes = charactername.encode("utf-8")
        unknown1 = 0
        unknown2 = 1
        unknown3 = 524287
        unknown4 = 0
        unknown5 = 0
        unknown6 = 50
        unknown7 = 0
        unknown8 = 200
        unknown9 = 200
        unknown10 = 200
        unknown11 = 50
        unknown12 = 50
        unknown13 = 50
        unknown14 = bytearray([0, 0, 0, 0])

        character_data = bytearray()
        character_data.extend(characterid.to_bytes(4, "little"))
        character_data.extend(name_bytes)
        character_data.extend(bytearray([0]*(3-len(name_bytes))))
        character_data.extend(unknown1.to_bytes(4, "little"))
        character_data.extend(unknown2.to_bytes(4, "little"))
        character_data.extend(unknown3.to_bytes(4, "little"))
        character_data.extend(unknown4.to_bytes(4, "little"))
        character_data.extend(unknown5.to_bytes(4, "little"))
        character_data.extend(unknown6.to_bytes(4, "little"))
        character_data.extend(unknown7.to_bytes(4, "little"))
        character_data.extend(unknown8.to_bytes(4, "little"))
        character_data.extend(unknown9.to_bytes(4, "little"))
        character_data.extend(unknown10.to_bytes(4, "little"))
        character_data.extend(unknown11.to_bytes(4, "little"))
        character_data.extend(unknown12.to_bytes(4, "little"))
        character_data.extend(unknown13.to_bytes(4, "little"))
        character_data.extend(unknown14)

        print(character_data)