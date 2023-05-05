class Iggy:

    def add_char_slot(filename, character_ID):
        # Open the file and read the data
        with open(filename, 'r+b') as f:
            data = bytearray(f.read())

            # Get the maximum number of slots from the file
            max_slots_offset = 0x2C
            max_slots = int.from_bytes(data[max_slots_offset:max_slots_offset+2], byteorder='little')

            # Check if there is still room for a new slot
            if max_slots < 800: #The value needs to be Hardcoded in the executable too, but I'll do it lather bah
                # Increment the maximum number of slots by 1
                max_slots += 1
                data[max_slots_offset:max_slots_offset+2] = max_slots.to_bytes(2, byteorder='little')

                # Insert a new character slot at the end
                char_start_offset = 0x19C9A
                char_end_offset = 0x19D75
                new_char_offset = char_end_offset + 1
                new_char_size = 4  # Size of the new character slot
                data[new_char_offset:new_char_offset+1] = 0x03  # Add the character initiator byte
                data[new_char_offset+1:new_char_offset+4] = character_ID.encode('utf-8')  # Replace 'xxx' with the character's three-letter ID
                data[new_char_offset+4] = 0x06  # Add the character terminator byte

                # Update the offsets of subsequent character slots
                slot_size = 5  # Size of each character slot
                for i in range(max_slots-1, 0, -1):
                    offset = char_start_offset + i*slot_size
                    new_offset = offset + new_char_size
                    data[new_offset: new_offset+slot_size] = data[offset:offset+slot_size]

                # Write the updated data back to the file
                f.seek(0)
                f.write(data)
                print('New character slot added successfully.')
            else:
                print('Maximum number of slots already reached.')
