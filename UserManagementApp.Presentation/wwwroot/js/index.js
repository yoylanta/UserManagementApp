document.addEventListener("DOMContentLoaded", () => {
    const selectAllCheckbox = document.getElementById("selectAll");
    const userCheckboxes = document.querySelectorAll(".user-checkbox");
    const blockButton = document.getElementById("blockUsers");
    const unblockButton = document.getElementById("unblockUsers");
    const deleteButton = document.getElementById("deleteUsers");
    const filterInput = document.getElementById("filterInput");

    // Select/Deselect all users
    selectAllCheckbox.addEventListener("change", () => {
        userCheckboxes.forEach(checkbox => {
            checkbox.checked = selectAllCheckbox.checked;
        });
    });

    // Filter users by name or email
    filterInput.addEventListener("input", () => {
        const filterValue = filterInput.value.toLowerCase();
        const rows = document.querySelectorAll("tbody tr");

        rows.forEach(row => {
            const name = row.cells[1].textContent.toLowerCase();
            const email = row.cells[2].textContent.toLowerCase();

            if (name.includes(filterValue) || email.includes(filterValue)) {
                row.style.display = "";
            } else {
                row.style.display = "none";
            }
        });
    });

    // Handle blocking users
    blockButton.addEventListener("click", async () => {
        const selectedUsers = getSelectedUsers();
        const { blocked, active } = getUserStatus(selectedUsers);

        let message = "";

        if (blocked.length > 0) {
            message += `Already blocked: ${getUsersNames(blocked)}.\n`;
        }

        if (active.length > 0) {
            try {
                const response = await fetch('/api/user/block', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(active)
                });

                if (!response.ok) {
                    showError('Failed to block users.');
                    return;
                }

                message += `<br> Successfully blocked: ${getUsersNames(active)}.`;

            } catch (error) {
                console.error(error);
                showError('An error occurred while blocking users.');
                return;
            }
        }

        if (message) {
            Swal.fire({
                icon: 'info',
                title: 'User Status Update',
                html: message,
                timer: 5000, 
                timerProgressBar: true
            }).then(() => location.reload());
        }
    });


    // Handle unblocking users
    unblockButton.addEventListener("click", async () => {
        const selectedUsers = getSelectedUsers();
        const { blocked, active } = getUserStatus(selectedUsers);

        let message = "";

        if (active.length > 0) {
            message += `Already active: ${getUsersNames(active)}.\n`;
        }

        if (blocked.length > 0) {
            try {
                const response = await fetch('/api/user/unblock', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(blocked)
                });

                if (!response.ok) {
                    showError('Failed to unblock users.');
                    return;
                }

                message += `<br> Successfully unblocked: ${getUsersNames(blocked)}.`;

            } catch (error) {
                console.error(error);
                showError('An error occurred while unblocking users.');
                return;
            }
        }

        if (message) {
            Swal.fire({
                icon: 'info',
                title: 'User Status Update',
                html: message,
                timer: 5000,
                timerProgressBar: true
            }).then(() => location.reload());
        }
    });

    // Handle deleting users
    deleteButton.addEventListener("click", async () => {
        const selectedUsers = getSelectedUsers();

        if (selectedUsers.length > 0) {
            const confirmed = await Swal.fire({
                icon: 'warning',
                title: 'Are you sure?',
                text: `Are you sure you want to delete ${selectedUsers.length} users?`,
                showCancelButton: true,
                confirmButtonText: 'Yes, delete it!',
                cancelButtonText: 'No, cancel!',
            });

            if (confirmed.isConfirmed) {
                try {
                    const response = await fetch('/api/user/delete', {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json'
                        },
                        body: JSON.stringify(selectedUsers)
                    });

                    if (response.ok) {
                        Swal.fire({
                            icon: 'success',
                            title: 'Deleted!',
                            text: `Successfully deleted users: ${getUsersNames(selectedUsers)}`,
                        }).then(() => location.reload());
                    } else {
                        showError('Failed to delete users.');
                    }
                } catch (error) {
                    console.error(error);
                    showError('An error occurred while deleting users.');
                }
            }
        }
    });

    function getSelectedUsers() {
        return Array.from(userCheckboxes)
            .filter(checkbox => checkbox.checked)
            .map(checkbox => parseInt(checkbox.dataset.userId));
    }

    function getUsersNames(userIds) {
        return userIds.map(userId => getUserNameById(userId)).join(", ");
    }

    function getUserNameById(userId) {
        const userNameElement = document.querySelector(`span[name-user-id="${userId}"]`);
        return userNameElement ? userNameElement.textContent.trim() : 'User not found';
    }

    function handleBlockError(responseData, selectedUsers) {
        if (responseData.message && responseData.message.includes("already blocked")) {
            Swal.fire({
                icon: 'warning',
                title: 'Some users are already blocked!',
                text: `The following users are already blocked: ${getUsersNames(selectedUsers)}`,
            });
        } else {
            showError('Failed to block users.');
        }
    }

    function showError(message) {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: message,
        });
    }
    function getUserStatus(userIds) {
        const users = document.querySelectorAll("span.badge");
        const userStatus = {
            blocked: [],
            active: []
        };

        users.forEach(user => {
            const userId = parseInt(user.getAttribute("block-user-id"));

            if (userIds.includes(userId)) {
                if (user.classList.contains("bg-danger")) {
                    userStatus.blocked.push(userId);
                } else {
                    userStatus.active.push(userId);
                }
            }
        });

        return userStatus;
    }

    function highlightCurrentUser() {
        const userHeading = document.querySelector("h3.display-7");
        if (!userHeading) return;

        const username = userHeading.textContent.split(", ")[1]?.trim();
        if (!username) return;

        document.querySelectorAll("td span[name-user-id]").forEach(span => {
            if (span.textContent.trim() === username) {
                const row = span.closest("tr");
                row.classList.add("current-user-row");
                row.addEventListener("mouseover", () => {
                    row.classList.add("hovered");
                });
                row.addEventListener("mouseout", () => {
                    row.classList.remove("hovered");
                });
                row.classList.add("glowing"); 
            }
        });
    }

    highlightCurrentUser();
});
